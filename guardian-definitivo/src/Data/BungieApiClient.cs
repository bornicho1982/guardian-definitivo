// guardian-definitivo/src/Data/BungieApiClient.cs
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using GuardianDefinitivo.Models; // Añadido para UserMembershipData
using GuardianDefinitivo.Models.User; // Añadido para BungieNetUser
using GuardianDefinitivo.Models.GroupV2; // Añadido para GroupUserInfoCard

namespace GuardianDefinitivo.Data
{
    public class BungieApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly BungieAuthConfig _authConfig;
        private readonly BungieOAuthHandler _oAuthHandler;
        private const string BaseUrl = "https://www.bungie.net/Platform";

        public BungieApiClient(HttpClient httpClient, BungieAuthConfig authConfig, BungieOAuthHandler oAuthHandler)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _authConfig = authConfig ?? throw new ArgumentNullException(nameof(authConfig));
            _oAuthHandler = oAuthHandler ?? throw new ArgumentNullException(nameof(oAuthHandler));
        }

        private async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string endpointPath)
        {
            var request = new HttpRequestMessage(method, $"{BaseUrl}{endpointPath}");
            request.Headers.Add("X-API-Key", _authConfig.BungieApiKey);

            var accessToken = await _oAuthHandler.GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            else
            {
                // Podríamos lanzar una excepción aquí si el token es estrictamente necesario
                // o permitir llamadas anónimas si la API de Bungie las soporta para ciertos endpoints.
                Console.WriteLine($"[APIClient] Advertencia: No hay token de acceso disponible para la solicitud a {endpointPath}.");
            }
            return request;
        }

        public async Task<T?> GetAsync<T>(string endpointPath) where T : class
        {
            try
            {
                using var request = await CreateRequestAsync(HttpMethod.Get, endpointPath);
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    // Bungie API envuelve todas las respuestas en un objeto "Response"
                    var bungieResponse = JsonSerializer.Deserialize<BungieApiResponse<T>>(jsonResponse);
                    if (bungieResponse?.ErrorCode == 1) // PlatformErrorCodes.Success
                    {
                        return bungieResponse.Response;
                    }
                    else
                    {
                        Console.WriteLine($"[APIClient] Error en la respuesta de Bungie API a {endpointPath}: {bungieResponse?.Message} (ErrorCode: {bungieResponse?.ErrorCode})");
                        // Aquí podríamos manejar errores específicos de la API de Bungie
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"[APIClient] Error HTTP: {response.StatusCode} al llamar a {endpointPath}");
                    // Considerar leer response.Content para más detalles del error si es necesario
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[APIClient] Excepción al llamar a {endpointPath}: {ex.Message}");
                return null;
            }
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpointPath, TRequest payload)
            where TRequest : class
            where TResponse : class
        {
            try
            {
                using var request = await CreateRequestAsync(HttpMethod.Post, endpointPath);
                var jsonPayload = JsonSerializer.Serialize(payload);
                request.Content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var bungieResponse = JsonSerializer.Deserialize<BungieApiResponse<TResponse>>(jsonResponse);
                     if (bungieResponse?.ErrorCode == 1) // PlatformErrorCodes.Success
                    {
                        return bungieResponse.Response;
                    }
                    else
                    {
                        Console.WriteLine($"[APIClient] Error en la respuesta de Bungie API a {endpointPath}: {bungieResponse?.Message} (ErrorCode: {bungieResponse?.ErrorCode})");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"[APIClient] Error HTTP: {response.StatusCode} al llamar a {endpointPath}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[APIClient] Excepción al llamar a {endpointPath} con POST: {ex.Message}");
                return null;
            }
        }

        // Método específico para GetLinkedProfiles
        // El endpoint es /User/GetMembershipsById/{membershipId}/{membershipType}/
        // O para el usuario actual (requiere OAuth): /User/GetMembershipsForCurrentUser/
        // Vamos a usar GetMembershipsForCurrentUser ya que tendremos OAuth.
        public async Task<UserMembershipData?> GetCurrentUserMembershipDataAsync() // Cambiado User.UserMembershipData a UserMembershipData
        {
            // El tipo de respuesta para GetMembershipsForCurrentUser es UserMembershipData (del namespace Models)
            // Ver: https://bungie-net.github.io/multi/schema_User-UserMembershipData.html#schema_User-UserMembershipData
            return await GetAsync<UserMembershipData>("/User/GetMembershipsForCurrentUser/");
        }
    }

    // Clase genérica para envolver las respuestas de la API de Bungie
    // Esta clase se mantiene aquí ya que es específica del API client.
    public class BungieApiResponse<T>
    {
        public T? Response { get; set; }
        public int ErrorCode { get; set; }
        public int ThrottleSeconds { get; set; }
        public string? ErrorStatus { get; set; }
        public string? Message { get; set; }
        public object? MessageData { get; set; } // Puede ser un objeto complejo
    }
}
// Las clases de modelo (UserMembershipData, BungieNetUserInfo, GroupUserInfoCard, BungieMembershipType)
// han sido movidas a sus respectivos archivos en la carpeta src/Models/ y sus subdirectorios.
// Los namespaces dentro de esas clases también han sido ajustados a GuardianDefinitivo.Models.*
