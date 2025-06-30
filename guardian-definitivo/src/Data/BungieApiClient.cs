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

        /// <summary>
        /// Gets all specified components for a Destiny profile.
        /// </summary>
        /// <param name="membershipType">Destiny membership type.</param>
        /// <param name="destinyMembershipId">Destiny membership ID.</param>
        /// <param name="components">A list of DestinyComponentType values indicating the components to retrieve.</param>
        /// <returns>The DestinyProfileResponse with the requested components.</returns>
        public async Task<Models.Destiny.Responses.DestinyProfileResponse?> GetDestinyProfileAsync(
            Models.Enums.BungieMembershipType membershipType,
            long destinyMembershipId,
            IEnumerable<DestinyComponentType> components)
        {
            if (components == null || !components.Any())
            {
                Console.WriteLine("[APIClient] Error: Al menos un componente debe ser especificado para GetDestinyProfileAsync.");
                return null;
            }
            // Ensure component numbers are distinct and join them with ","
            string componentsQuery = string.Join(",", components.Select(c => (int)c).Distinct());
            string endpointPath = $"/Destiny2/{(int)membershipType}/Profile/{destinyMembershipId}/?components={componentsQuery}";

            // Loguear el endpoint para depuración
            // Console.WriteLine($"[APIClient] Calling GetDestinyProfileAsync endpoint: {BaseUrl}{endpointPath}");

            return await GetAsync<Models.Destiny.Responses.DestinyProfileResponse>(endpointPath);
        }
    }

    /// <summary>
    /// Represents the component types that can be requested from the Destiny API.
    /// Values correspond to Destiny.DestinyComponentType in the API.
    /// Full list: https://bungie-net.github.io/multi/schema_Destiny-DestinyComponentType.html#schema_Destiny-DestinyComponentType
    /// </summary>
    public enum DestinyComponentType
    {
        None = 0,
        // Profile-level components
        Profiles = 100,
        VendorReceipts = 101,
        ProfileInventories = 102, // Vault
        ProfileCurrencies = 103,
        PlatformSilver = 104,
        Kiosks = 105,
        ProfilePlugSets = 106,
        ProfileProgression = 107,
        ProfilePresentationNodes = 108,
        ProfileRecords = 109,
        ProfileCollectibles = 110,
        ProfileTransitoryData = 111,
        Metrics = 112,
        ProfileStringVariables = 113,
        ProfileCommendations = 114,

        // Character-level components
        Characters = 200,
        CharacterInventories = 201, // Character Inventory (un-equipped)
        CharacterProgressions = 202,
        CharacterRenderData = 203,
        CharacterActivities = 204,
        CharacterEquipment = 205, // Character Equipment (equipped)
        CharacterKiosks = 206,
        CharacterPlugSets = 207,
        CharacterUninstancedItemComponents = 208,
        CharacterPresentationNodes = 209,
        CharacterRecords = 210,
        CharacterCollectibles = 211,
        CharacterStringVariables = 212,
        CharacterCraftables = 213,
        CharacterLoadouts = 214,
        CharacterCurrencyLookups = 215,

        // Item instance components (data for items returned by the above)
        ItemInstances = 300,
        ItemObjectives = 301,
        ItemPerks = 302,
        ItemRenderData = 303,
        ItemStats = 304,
        ItemSockets = 305,
        ItemTalentGrids = 306,
        // ItemCommonData = 307, // Not typically requested directly with GetProfile in this way
        ItemPlugStates = 308, // Usually part of Sockets
        ItemPlugObjectives = 309, // Usually part of Sockets
        ItemReusablePlugs = 310 // Usually part of Sockets
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
