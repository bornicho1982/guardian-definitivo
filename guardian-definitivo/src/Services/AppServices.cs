// guardian-definitivo/src/Services/AppServices.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GuardianDefinitivo.Data;
using GuardianDefinitivo.Models; // For UserMembershipData
using GuardianDefinitivo.Models.Enums; // For BungieMembershipType
using GuardianDefinitivo.Models.Destiny.Responses; // For DestinyProfileResponse

namespace GuardianDefinitivo.Services
{
    public class AppServices
    {
        private static readonly HttpClient httpClient = new HttpClient(); // Reutilizar HttpClient

        public BungieAuthConfig? AuthConfig { get; private set; }
        public BungieOAuthHandler? OAuthHandler { get; private set; }
        public BungieApiClient? ApiClient { get; private set; }
        public ManifestService? ManifestService { get; private set; }

        public UserMembershipData? CurrentUserMembershipData { get; private set; }
        public GroupV2.GroupUserInfoCard? PrimaryDestinyProfile { get; private set; }
        public DestinyProfileResponse? CurrentDestinyProfileResponse { get; private set; }

        public bool IsInitialized { get; private set; } = false;
        public bool IsAuthenticated => OAuthHandler?.GetAccessTokenAsync().Result != null; // Simplificado para el ejemplo

        public async Task InitializeAsync()
        {
            Console.WriteLine("[AppServices] Inicializando servicios...");

            // 1. Cargar Configuración
            AuthConfig = BungieAuthConfig.Load();
            if (AuthConfig == null || string.IsNullOrEmpty(AuthConfig.BungieApiKey) || string.IsNullOrEmpty(AuthConfig.OAuthClientId))
            {
                Console.WriteLine("[AppServices] Error: No se pudo cargar la configuración o falta API Key/Client ID. Verifica AppConfig.json.");
                // Podríamos lanzar una excepción aquí o manejarlo de otra forma
                return;
            }
            Console.WriteLine("[AppServices] Configuración cargada.");

            // 2. Instanciar Handlers y Clients
            OAuthHandler = new BungieOAuthHandler(AuthConfig, httpClient);
            ApiClient = new BungieApiClient(httpClient, AuthConfig, OAuthHandler);
            ManifestService = new ManifestService(ApiClient, httpClient);

            // 3. Inicializar Manifest
            Console.WriteLine("[AppServices] Inicializando ManifestService...");
            await ManifestService.InitializeAsync();
            Console.WriteLine("[AppServices] ManifestService inicializado.");

            IsInitialized = true;
            Console.WriteLine("[AppServices] Servicios inicializados.");
        }

        public async Task<bool> AuthenticateAsync()
        {
            if (!IsInitialized || OAuthHandler == null)
            {
                Console.WriteLine("[AppServices] Servicios no inicializados. Llama a InitializeAsync() primero.");
                return false;
            }

            string? accessToken = await OAuthHandler.GetAccessTokenAsync();
            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("[AppServices] Iniciando autenticación OAuth...");
                string authUrl = OAuthHandler.GetAuthorizationUrl();
                Console.WriteLine($"[AppServices] Por favor, visita esta URL en tu navegador para autorizar la aplicación:");
                Console.WriteLine(authUrl);
                Console.WriteLine();
                Console.WriteLine("[AppServices] Después de autorizar, Bungie te redirigirá a una URL (probablemente localhost).");
                Console.WriteLine("[AppServices] Copia el valor del parámetro 'code' de esa URL de redirección y pégalo aquí:");
                Console.Write("Introduce el código de autorización: ");
                string? authorizationCode = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(authorizationCode))
                {
                    Console.WriteLine("[AppServices] No se introdujo ningún código. Autenticación fallida.");
                    return false;
                }
                // En una app real, el state se generaría y validaría.
                bool tokenObtained = await OAuthHandler.HandleCallbackAsync(authorizationCode.Trim(), "dummy_console_state");
                if (!tokenObtained)
                {
                    Console.WriteLine("[AppServices] No se pudo obtener el token de acceso. Verifica el código o la configuración.");
                    return false;
                }
                Console.WriteLine("[AppServices] Token de acceso obtenido con éxito.");
                return true;
            }
            Console.WriteLine("[AppServices] Ya autenticado o token refrescado.");
            return true;
        }

        public async Task<bool> LoadUserProfileAsync()
        {
            if (!IsAuthenticated || ApiClient == null)
            {
                Console.WriteLine("[AppServices] No autenticado o ApiClient no disponible.");
                return false;
            }

            Console.WriteLine("[AppServices] Obteniendo datos del perfil de Bungie.net...");
            CurrentUserMembershipData = await ApiClient.GetCurrentUserMembershipDataAsync();

            if (CurrentUserMembershipData != null && CurrentUserMembershipData.bungieNetUser != null)
            {
                Console.WriteLine($"[AppServices] Perfil de Bungie.net cargado para: {CurrentUserMembershipData.bungieNetUser.displayName}");
                DeterminePrimaryDestinyProfile();
                return true;
            }
            else
            {
                Console.WriteLine("[AppServices] No se pudo obtener la información del perfil de Bungie.net.");
                return false;
            }
        }

        private void DeterminePrimaryDestinyProfile()
        {
            if (CurrentUserMembershipData?.destinyMemberships == null || !CurrentUserMembershipData.destinyMemberships.Any())
            {
                PrimaryDestinyProfile = null;
                return;
            }

            if (CurrentUserMembershipData.primaryMembershipId.HasValue)
            {
                PrimaryDestinyProfile = CurrentUserMembershipData.destinyMemberships.FirstOrDefault(
                    p => p.membershipId == CurrentUserMembershipData.primaryMembershipId.Value);
            }

            if (PrimaryDestinyProfile == null)
            {
                PrimaryDestinyProfile = CurrentUserMembershipData.destinyMemberships.FirstOrDefault();
            }
            Console.WriteLine($"[AppServices] Perfil de Destiny primario/seleccionado: {PrimaryDestinyProfile?.displayName} ({PrimaryDestinyProfile?.membershipType})");
        }

        public async Task<bool> LoadDestinyInventoryAsync()
        {
            if (PrimaryDestinyProfile == null || ApiClient == null)
            {
                Console.WriteLine("[AppServices] Perfil de Destiny primario no determinado o ApiClient no disponible.");
                return false;
            }

            Console.WriteLine($"[AppServices] Cargando inventario para: {PrimaryDestinyProfile.displayName}...");
            var componentsToFetch = new List<DestinyComponentType>
            {
                DestinyComponentType.Profiles,
                DestinyComponentType.Characters,
                DestinyComponentType.CharacterEquipment,
                DestinyComponentType.CharacterInventories,
                DestinyComponentType.ProfileInventories,
                DestinyComponentType.ItemInstances,
                DestinyComponentType.ItemSockets,
                DestinyComponentType.ItemPerks,
                DestinyComponentType.ItemStats,
                DestinyComponentType.ItemReusablePlugs,
                DestinyComponentType.ItemPlugObjectives
            };

            CurrentDestinyProfileResponse = await ApiClient.GetDestinyProfileAsync(
                PrimaryDestinyProfile.membershipType,
                PrimaryDestinyProfile.membershipId,
                componentsToFetch);

            if (CurrentDestinyProfileResponse != null)
            {
                Console.WriteLine("[AppServices] Datos del inventario de Destiny cargados con éxito.");
                return true;
            }
            else
            {
                Console.WriteLine("[AppServices] No se pudieron cargar los datos del inventario de Destiny.");
                return false;
            }
        }
    }
}
