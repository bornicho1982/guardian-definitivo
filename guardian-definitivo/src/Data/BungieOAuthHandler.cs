// guardian-definitivo/src/Data/BungieOAuthHandler.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GuardianDefinitivo.Data
{
    public class BungieOAuthHandler
    {
        private readonly BungieAuthConfig _config;
        private readonly HttpClient _httpClient;
        private string? _accessToken;
        private string? _refreshToken;
        private DateTime _tokenExpirationTime;

        public BungieOAuthHandler(BungieAuthConfig config, HttpClient httpClient)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public string GetAuthorizationUrl()
        {
            // TODO: Construir la URL de autorización de Bungie.net
            // Ejemplo: https://www.bungie.net/en/OAuth/Authorize?client_id={_config.OAuthClientId}&response_type=code&state=some_random_state
            // Es importante incluir un parámetro 'state' para mitigar CSRF y verificarlo en el callback.
            var state = Guid.NewGuid().ToString(); // Generar un estado aleatorio
            // Guardar 'state' en algún lugar para verificarlo después (ej. memoria, caché, etc.)
            Console.WriteLine($"[OAuth] Generated state: {state}");
            return $"https://www.bungie.net/en/OAuth/Authorize?client_id={_config.OAuthClientId}&response_type=code&state={state}";
        }

        public async Task<bool> HandleCallbackAsync(string authorizationCode, string receivedState)
        {
            // TODO: Verificar que 'receivedState' coincide con el 'state' generado.
            // TODO: Intercambiar el código de autorización por un token de acceso.
            //       Hacer una petición POST a https://www.bungie.net/platform/app/oauth/token/
            //       con grant_type=authorization_code, code={authorizationCode}, client_id, client_secret.
            Console.WriteLine($"[OAuth] Handling callback with code: {authorizationCode} and state: {receivedState}");

            var tokenRequestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", authorizationCode),
                new KeyValuePair<string, string>("client_id", _config.OAuthClientId),
                // NOTA: Bungie.net puede requerir que client_secret se envíe en la cabecera Basic Auth para confidencial clients.
                // Para aplicaciones públicas (como esta, que se ejecuta en el cliente), client_secret no se usa aquí o es opcional.
                // Revisar documentación de Bungie: https://bungie-net.github.io/multi/oauth_notes/oauth_doc.html#oauth-step-2-request-access-token
                // new KeyValuePair<string, string>("client_secret", _config.OAuthClientSecret), // Puede no ser necesario o ir en Basic Auth
            });

            // Si se requiere Basic Auth para client_id y client_secret:
            // var byteArray = System.Text.Encoding.ASCII.GetBytes($"{_config.OAuthClientId}:{_config.OAuthClientSecret}");
            // _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync("https://www.bungie.net/platform/app/oauth/token/", tokenRequestContent);
                response.EnsureSuccessStatusCode(); // Lanza excepción si no es exitoso

                string responseBody = await response.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<OAuthTokenResponse>(responseBody);

                if (tokenData != null && !string.IsNullOrEmpty(tokenData.access_token))
                {
                    _accessToken = tokenData.access_token;
                    _refreshToken = tokenData.refresh_token;
                    _tokenExpirationTime = DateTime.UtcNow.AddSeconds(tokenData.expires_in);
                    Console.WriteLine($"[OAuth] Access token obtained. Expires in: {tokenData.expires_in}s");
                    return true;
                }
                else
                {
                    Console.WriteLine($"[OAuth] Error: No se pudo obtener el token de acceso. Respuesta: {responseBody}");
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[OAuth] Error en la solicitud de token: {e.Message}");
                // Aquí podrías leer e.Response.Content si necesitas más detalles del error de la API
                return false;
            }
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            if (_accessToken != null && DateTime.UtcNow < _tokenExpirationTime.AddMinutes(-5)) // Refrescar 5 mins antes de expirar
            {
                return _accessToken;
            }

            if (!string.IsNullOrEmpty(_refreshToken))
            {
                return await RefreshAccessTokenAsync();
            }

            Console.WriteLine("[OAuth] No hay token de acceso válido ni token de refresco.");
            return null;
        }

        private async Task<string?> RefreshAccessTokenAsync()
        {
            // TODO: Implementar la lógica para refrescar el token de acceso usando _refreshToken.
            //       Hacer una petición POST a https://www.bungie.net/platform/app/oauth/token/
            //       con grant_type=refresh_token, refresh_token={_refreshToken}, client_id, client_secret.
            Console.WriteLine("[OAuth] Refrescando token de acceso...");
            if (string.IsNullOrEmpty(_refreshToken))
            {
                Console.WriteLine("[OAuth] Error: No hay token de refresco disponible.");
                return null;
            }

            var refreshTokenRequestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", _refreshToken),
                new KeyValuePair<string, string>("client_id", _config.OAuthClientId),
                // new KeyValuePair<string, string>("client_secret", _config.OAuthClientSecret), // Ver nota en HandleCallbackAsync
            });

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync("https://www.bungie.net/platform/app/oauth/token/", refreshTokenRequestContent);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<OAuthTokenResponse>(responseBody);

                if (tokenData != null && !string.IsNullOrEmpty(tokenData.access_token))
                {
                    _accessToken = tokenData.access_token;
                    // Bungie puede o no devolver un nuevo refresh token. Si lo hace, actualizarlo.
                    _refreshToken = tokenData.refresh_token ?? _refreshToken;
                    _tokenExpirationTime = DateTime.UtcNow.AddSeconds(tokenData.expires_in);
                    Console.WriteLine($"[OAuth] Token de acceso refrescado. Nuevo refresh token: {!string.IsNullOrEmpty(tokenData.refresh_token)}");
                    return _accessToken;
                }
                else
                {
                    Console.WriteLine($"[OAuth] Error: No se pudo refrescar el token de acceso. Respuesta: {responseBody}");
                    _accessToken = null; // Invalidar token actual
                    _refreshToken = null; // Invalidar refresh token, forzar re-login
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"[OAuth] Error en la solicitud de refresco de token: {e.Message}");
                _accessToken = null;
                _refreshToken = null;
                return null;
            }
        }

        public void Logout()
        {
            _accessToken = null;
            _refreshToken = null;
            _tokenExpirationTime = DateTime.MinValue;
            Console.WriteLine("[OAuth] Sesión cerrada.");
            // TODO: Opcionalmente, se podría intentar invalidar el token en el servidor de Bungie si la API lo permite.
        }
    }

    // Clase auxiliar para deserializar la respuesta del token OAuth
    internal class OAuthTokenResponse
    {
        public string? access_token { get; set; }
        public string? token_type { get; set; } // "Bearer"
        public int expires_in { get; set; } // Segundos
        public string? refresh_token { get; set; }
        public int refresh_expires_in { get; set; } // Segundos
        public string? membership_id { get; set; }
    }
}
