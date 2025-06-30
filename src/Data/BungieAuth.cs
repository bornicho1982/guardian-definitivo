using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using GuardianDefinitivo.Data.Models; // Asegúrate que este using esté presente o sea correcto
using System.IO; // Para Path.Combine y File.Exists
using System; // Para Exception

namespace GuardianDefinitivo.Data
{
    // Excepción personalizada para errores de configuración
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message) : base(message) { }
    }

    public class BungieAuth
    {
        private readonly HttpClient http = new HttpClient();

        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string apiKey;
        private readonly string redirectUri;

        public BungieAuth()
        {
            string secretsFilePath = "secrets.json"; // Podría ser Path.Combine para más robustez si la CWD varía
            if (!File.Exists(secretsFilePath))
            {
                throw new ConfigurationException($"Error: El archivo '{secretsFilePath}' no fue encontrado. Asegúrate de que exista y contenga las credenciales de la API de Bungie.");
            }

            var json = File.ReadAllText(secretsFilePath);
            var secrets = JsonSerializer.Deserialize<BungieSecrets>(json);

            if (secrets?.Bungie == null)
            {
                throw new ConfigurationException($"Error: El archivo '{secretsFilePath}' no contiene la sección 'Bungie' esperada o está malformado.");
            }

            clientId = secrets.Bungie.ClientId ?? throw new ConfigurationException("Error: 'ClientId' no encontrado en secrets.json.");
            clientSecret = secrets.Bungie.ClientSecret ?? throw new ConfigurationException("Error: 'ClientSecret' no encontrado en secrets.json.");
            apiKey = secrets.Bungie.ApiKey ?? throw new ConfigurationException("Error: 'ApiKey' no encontrado en secrets.json.");
            redirectUri = secrets.Bungie.RedirectUri ?? throw new ConfigurationException("Error: 'RedirectUri' no encontrado en secrets.json.");

            if (string.IsNullOrWhiteSpace(clientId))
                throw new ConfigurationException("Error: 'ClientId' en secrets.json no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new ConfigurationException("Error: 'ClientSecret' en secrets.json no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ConfigurationException("Error: 'ApiKey' en secrets.json no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(redirectUri))
                throw new ConfigurationException("Error: 'RedirectUri' en secrets.json no puede estar vacío.");
        }

        public async Task<string?> AutenticarAsync()
        {
            // Validar que redirectUri sea una URI válida antes de usarla con HttpListener
            if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out var parsedRedirectUri) ||
                (parsedRedirectUri.Scheme != Uri.UriSchemeHttp && parsedRedirectUri.Scheme != Uri.UriSchemeHttps))
            {
                Console.WriteLine($"❌ Error: El RedirectUri '{redirectUri}' en secrets.json no es una URI válida para el listener HTTP. Debe ser http o https.");
                // O lanzar una ConfigurationException aquí también
                // throw new ConfigurationException($"Error: El RedirectUri '{redirectUri}' en secrets.json no es una URI válida para el listener HTTP. Debe ser http o https y terminar en '/'.");
                // Por ahora, solo log y retorno null para evitar crash si el usuario no puede cambiarlo inmediatamente.
                 return null;
            }
            // HttpListener requiere que el prefijo termine en '/'
            string listenerPrefix = redirectUri.EndsWith("/") ? redirectUri : redirectUri + "/";


            string authUrl = $"https://www.bungie.net/en/OAuth/Authorize?client_id={clientId}&response_type=code&state=guardian_definitivo_auth"; // state mejorado
            Console.WriteLine("🔓 Abriendo navegador para iniciar sesión...");
            Process.Start(new ProcessStartInfo { FileName = authUrl, UseShellExecute = true });

            var listener = new HttpListener();
            // Asegurarse que el prefix sea válido para HttpListener.
            // Comúnmente localhost o una IP específica.
            // Ejemplo: listener.Prefixes.Add("http://localhost:8888/oauth-callback/");
            // El redirectUri de Bungie debe coincidir con esto.
            listener.Prefixes.Add(listenerPrefix);

            try
            {
                listener.Start();
                Console.WriteLine($"🎧 Escuchando en {listenerPrefix} para la respuesta de Bungie...");

                var context = await listener.GetContextAsync();
                var code = context.Request.QueryString["code"];
                var state = context.Request.QueryString["state"];

                // Validar el state para prevenir CSRF
                if (state != "guardian_definitivo_auth")
                {
                    Console.WriteLine("❌ Error: El parámetro 'state' de la respuesta OAuth no coincide. Posible ataque CSRF.");
                    byte[] errorMsg = Encoding.UTF8.GetBytes("<h1>❌ Error de autenticación: state inválido.</h1>");
                    context.Response.OutputStream.Write(errorMsg);
                    context.Response.Close();
                    return null;
                }

                byte[] msg = Encoding.UTF8.GetBytes("<h1>✅ Autenticación completada. Puedes cerrar esta ventana.</h1>");
                context.Response.OutputStream.Write(msg);
                context.Response.Close();

                if (string.IsNullOrEmpty(code))
                {
                    Console.WriteLine("❌ Error: No se recibió el código de autorización de Bungie.");
                    return null;
                }

                return await ObtenerAccessToken(code);
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine($"❌ Error de HttpListener: {ex.Message}");
                Console.WriteLine("Asegúrate de que la aplicación tenga permisos para escuchar en el puerto especificado y que el puerto no esté en uso.");
                Console.WriteLine($"Intenta ejecutar la aplicación como administrador o usa 'netsh http add urlacl url={listenerPrefix} user=TU_USUARIO'.");
                return null;
            }
            finally
            {
                if(listener.IsListening)
                    listener.Stop();
            }
        }

        private async Task<string?> ObtenerAccessToken(string code)
        {
            var datos = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "client_id", clientId }, // Ya no es nullable
                { "client_secret", clientSecret }, // Ya no es nullable
                { "redirect_uri", redirectUri } // Ya no es nullable
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://www.bungie.net/Platform/App/OAuth/Token/");
            request.Content = new FormUrlEncodedContent(datos);
            // El X-API-Key no es necesario para el endpoint /App/OAuth/Token/ si se usa Basic Auth, pero puede ser necesario para otros.
            // Bungie espera las credenciales del cliente (clientId:clientSecret) como Basic Auth header o en el body.
            // Aquí lo estamos enviando en el body. Si se necesitara Basic Auth:
            // var basicAuthValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            // request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuthValue);
            // Sin embargo, el SDK de Bungie y la práctica común para este endpoint es enviar client_id y client_secret en el body.
            // Si se incluye X-API-Key aquí, no debería causar problemas, pero es redundante para este endpoint específico.
            request.Headers.Add("X-API-Key", apiKey);


            var response = await http.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Error al obtener token de acceso ({response.StatusCode}): {body}");
                return null;
            }

            var token = JsonSerializer.Deserialize<OAuthTokenResponse>(body);
            if (token?.access_token == null)
            {
                Console.WriteLine("❌ Error: La respuesta del token de acceso no contenía un access_token.");
                return null;
            }

            // Considerar almacenar el refresh_token y expires_in de forma segura también
            File.WriteAllText("token.json", JsonSerializer.Serialize(token, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine("✅ Token almacenado en 'token.json'");
            return token.access_token;
        }

        // OAuthTokenResponse se mantiene igual, podría moverse a Models si se usa en otros sitios.
        private class OAuthTokenResponse
        {
            public string? access_token { get; set; }
            public string? token_type { get; set; } // Añadido por completitud, común en OAuth
            public int expires_in { get; set; }
            public string? refresh_token { get; set; }
            public long? membership_id { get; set; } // Bungie devuelve esto como string, pero puede ser long
        }
    }
}