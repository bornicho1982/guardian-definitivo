using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace GuardianDefinitivo.Data
{
    public class BungieAuth
    {
        private readonly HttpClient http = new HttpClient();

        private string? clientId;
        private string? clientSecret;
        private string? apiKey;
        private string? redirectUri;

        public BungieAuth()
        {
            // Cargar secrets.json
            var json = File.ReadAllText("secrets.json");
            var datos = JsonSerializer.Deserialize<SecretsFile>(json);
            clientId = datos?.Bungie?.ClientId;
            clientSecret = datos?.Bungie?.ClientSecret;
            apiKey = datos?.Bungie?.ApiKey;
            redirectUri = datos?.Bungie?.RedirectUri;
        }

        public async Task<string?> AutenticarAsync()
        {
            string authUrl = $"https://www.bungie.net/en/OAuth/Authorize?client_id={clientId}&response_type=code&state=xyz";
            Console.WriteLine("🔓 Abriendo navegador para iniciar sesión...");
            Process.Start(new ProcessStartInfo { FileName = authUrl, UseShellExecute = true });

            var listener = new HttpListener();
            listener.Prefixes.Add($"{redirectUri}/");
            listener.Start();

            var context = await listener.GetContextAsync();
            var code = context.Request.QueryString["code"];
            byte[] msg = Encoding.UTF8.GetBytes("<h1>✅ Autenticación completada. Puedes cerrar esta ventana.</h1>");
            context.Response.OutputStream.Write(msg);
            context.Response.Close();
            listener.Stop();

            if (string.IsNullOrEmpty(code)) return null;

            return await ObtenerAccessToken(code);
        }

        private async Task<string?> ObtenerAccessToken(string code)
        {
            var datos = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "client_id", clientId ?? "" },
                { "client_secret", clientSecret ?? "" },
                { "redirect_uri", redirectUri ?? "" }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://www.bungie.net/Platform/App/OAuth/Token/");
            request.Content = new FormUrlEncodedContent(datos);
            request.Headers.Add("X-API-Key", apiKey);

            var response = await http.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("❌ Error: " + body);
                return null;
            }

            var token = JsonSerializer.Deserialize<OAuthTokenResponse>(body);
            File.WriteAllText("token.json", JsonSerializer.Serialize(token, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine("✅ Token almacenado en 'token.json'");
            return token?.access_token;
        }

        private class SecretsFile
        {
            public BungieInfo? Bungie { get; set; }
        }

        private class BungieInfo
        {
            public string? ClientId { get; set; }
            public string? ClientSecret { get; set; }
            public string? ApiKey { get; set; }
            public string? RedirectUri { get; set; }
        }

        private class OAuthTokenResponse
        {
            public string? access_token { get; set; }
            public string? refresh_token { get; set; }
            public int expires_in { get; set; }
            public string? membership_id { get; set; }
        }
    }
}