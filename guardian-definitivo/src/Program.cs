// Archivo de entrada principal para la aplicación Guardián Definitivo
using System;
using System.Net.Http;
using System.Threading.Tasks;
using GuardianDefinitivo.Data;
using GuardianDefinitivo.Models;

public class Program
{
    // HttpClient debe ser instanciado una vez y reutilizado
    private static readonly HttpClient httpClient = new HttpClient();
    private static ManifestService? manifestService; // Para acceder al Manifest

    public static async Task Main(string[] args)
    {
        Console.WriteLine("¡Bienvenido a Guardián Definitivo!");

        // 1. Cargar la configuración de la aplicación
        Console.WriteLine("[Main] Cargando configuración...");
        var authConfig = BungieAuthConfig.Load();
        if (authConfig == null || string.IsNullOrEmpty(authConfig.BungieApiKey) || string.IsNullOrEmpty(authConfig.OAuthClientId))
        {
            Console.WriteLine("[Main] Error: No se pudo cargar la configuración o falta API Key/Client ID. Verifica AppConfig.json.");
            Console.WriteLine("[Main] Asegúrate de reemplazar 'TU_API_KEY_AQUI', 'TU_CLIENT_ID_AQUI', etc., en guardian-definitivo/src/AppConfig.json");
            return;
        }
        Console.WriteLine("[Main] Configuración cargada.");

        // 2. Instanciar BungieOAuthHandler y BungieApiClient
        var oAuthHandler = new BungieOAuthHandler(authConfig, httpClient);
        var apiClient = new BungieApiClient(httpClient, authConfig, oAuthHandler);

        // 2.5 Instanciar y preparar ManifestService
        Console.WriteLine("[Main] Inicializando ManifestService...");
        manifestService = new ManifestService(apiClient, httpClient);
        await manifestService.InitializeAsync(); // Descarga/actualiza el manifest si es necesario
        Console.WriteLine("[Main] ManifestService inicializado.");

        // 3. Simular flujo de autenticación OAuth
        string? accessToken = await oAuthHandler.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(accessToken))
        {
            Console.WriteLine("[Main] Iniciando autenticación OAuth...");
            string authUrl = oAuthHandler.GetAuthorizationUrl();
            Console.WriteLine($"[Main] Por favor, visita esta URL en tu navegador para autorizar la aplicación:");
            Console.WriteLine(authUrl);
            Console.WriteLine();
            Console.WriteLine("[Main] Después de autorizar, Bungie te redirigirá a una URL (probablemente localhost).");
            Console.WriteLine("[Main] Copia el valor del parámetro 'code' de esa URL de redirección y pégalo aquí:");
            Console.Write("Introduce el código de autorización: ");
            string? authorizationCode = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(authorizationCode))
            {
                Console.WriteLine("[Main] No se introdujo ningún código. Saliendo.");
                return;
            }

            // Aquí también necesitaríamos el parámetro 'state' si lo estuviéramos validando seriamente.
            // Por simplicidad en este ejemplo de consola, lo omitimos, pero en una app real es crucial.
            bool tokenObtained = await oAuthHandler.HandleCallbackAsync(authorizationCode.Trim(), "dummy_state_for_console_app");

            if (!tokenObtained)
            {
                Console.WriteLine("[Main] No se pudo obtener el token de acceso. Verifica el código o la configuración.");
                return;
            }
            Console.WriteLine("[Main] Token de acceso obtenido con éxito.");
        }
        else
        {
            Console.WriteLine("[Main] Usando token de acceso existente/refrescado.");
        }

        // 4. Obtener y mostrar información del perfil
        Console.WriteLine("[Main] Obteniendo datos del perfil de Bungie.net...");
        var membershipData = await apiClient.GetCurrentUserMembershipDataAsync();

        if (membershipData != null && membershipData.bungieNetUser != null)
        {
            Console.WriteLine($"[Main] ¡Hola, {membershipData.bungieNetUser?.displayName} (ID: {membershipData.bungieNetUser?.membershipId})!");
            if (membershipData.destinyMemberships != null && membershipData.destinyMemberships.Count > 0)
            {
                Console.WriteLine("[Main] Perfiles de Destiny 2 vinculados:");
                Models.GroupV2.GroupUserInfoCard? primaryDestinyProfile = null;

                if (membershipData.primaryMembershipId.HasValue)
                {
                    primaryDestinyProfile = membershipData.destinyMemberships.FirstOrDefault(p => p.membershipId == membershipData.primaryMembershipId.Value);
                    Console.WriteLine($"[Main] ID de membresía primaria de Destiny: {membershipData.primaryMembershipId.Value}");
                }

                if (primaryDestinyProfile == null)
                {
                    primaryDestinyProfile = membershipData.destinyMemberships.FirstOrDefault();
                    if (primaryDestinyProfile != null)
                    {
                         Console.WriteLine($"[Main] No se encontró membresía primaria explícita, se usará la primera disponible: {primaryDestinyProfile.membershipId} ({primaryDestinyProfile.membershipType})");
                    }
                }

                foreach (var profile in membershipData.destinyMemberships)
                {
                    Console.WriteLine($"  - Plataforma: {profile.membershipType} (ID: {profile.membershipId}) {(profile.membershipId == primaryDestinyProfile?.membershipId ? "[PRIMARIO/SELECCIONADO]" : "")}");
                    Console.WriteLine($"    Nombre Global: {profile.bungieGlobalDisplayName}#{profile.bungieGlobalDisplayNameCode}");
                    Console.WriteLine($"    Nombre en Plataforma: {profile.displayName}");
                }

                if (primaryDestinyProfile != null)
                {
                    Console.WriteLine($"[Main] Intentando cargar inventario para el perfil: {primaryDestinyProfile.displayName} ({primaryDestinyProfile.membershipId} en {primaryDestinyProfile.membershipType})...");

                    var componentsToFetch = new List<DestinyComponentType>
                    {
                        DestinyComponentType.Profiles,          // Para info general del perfil Destiny
                        DestinyComponentType.Characters,        // Para obtener IDs y datos básicos de personajes
                        DestinyComponentType.CharacterEquipment,// Para items equipados
                        DestinyComponentType.CharacterInventories, // Para items en inventario de personaje
                        DestinyComponentType.ProfileInventories, // Para la Bóveda
                        DestinyComponentType.ItemInstances,     // Stats de instancia (nivel de luz, etc.)
                        DestinyComponentType.ItemSockets,       // Sockets y mods
                        DestinyComponentType.ItemPerks,         // Perks de items
                        DestinyComponentType.ItemStats          // Stats base de items
                    };

                    var destinyProfileResponse = await apiClient.GetDestinyProfileAsync(primaryDestinyProfile.membershipType, primaryDestinyProfile.membershipId, componentsToFetch);

                    if (destinyProfileResponse != null)
                    {
                        Console.WriteLine("[Main] ¡Datos del perfil de Destiny obtenidos con éxito!");

                        // Mostrar información de la bóveda (ProfileInventories)
                        if (destinyProfileResponse.ProfileInventory?.Data?.Items != null)
                        {
                            Console.WriteLine($"[Main] Items en la Bóveda: {destinyProfileResponse.ProfileInventory.Data.Items.Count}");
                        }

                        // Mostrar información del primer personaje (si existe)
                        if (destinyProfileResponse.Characters?.Data != null && destinyProfileResponse.Characters.Data.Any())
                        {
                            var firstCharacter = destinyProfileResponse.Characters.Data.First();
                            long firstCharacterId = firstCharacter.Key;
                            var characterData = firstCharacter.Value;

                            Console.WriteLine($"[Main] --- Personaje Principal ({characterData.CharacterId}) ---");
                            // Aquí podríamos usar el Manifest para obtener Class, Race, Gender
                            Console.WriteLine($"  Luz: {characterData.Light}, Nivel: {characterData.BaseCharacterLevel}");
                            Console.WriteLine($"  Tiempo jugado: {characterData.MinutesPlayedTotal} minutos");

                            // Items equipados
                            if (destinyProfileResponse.CharacterEquipment?.Data?.TryGetValue(firstCharacterId, out var equippedItems) == true && equippedItems.Items != null)
                            {
                                Console.WriteLine("  Items Equipados (primeros 5):");
                                if (manifestService != null) // Asegurarse que el manifest está disponible
                                {
                                    foreach (var item in equippedItems.Items.Take(5))
                                    {
                                        var itemDef = await manifestService.GetInventoryItemDefinitionAsync(item.ItemHash);
                                        string itemName = itemDef?.DisplayProperties?.Name ?? $"Item Hash: {item.ItemHash}";
                                        string itemType = itemDef?.ItemTypeDisplayName ?? "Desconocido";

                                        Console.Write($"    - {itemName} ({itemType}) (Instancia: {item.ItemInstanceId?.ToString() ?? "N/A"})");
                                        if (item.ItemInstanceId.HasValue && destinyProfileResponse.ItemComponents?.Instances?.Data?.TryGetValue(item.ItemInstanceId.Value, out var instanceData) == true)
                                        {
                                            Console.Write($", Luz: {instanceData.ItemLevel}");
                                        }
                                        Console.WriteLine();
                                    }
                                }
                                else
                                {
                                    // Fallback si el manifest no está disponible
                                    foreach (var item in equippedItems.Items.Take(5))
                                    {
                                        Console.WriteLine($"    - ItemHash: {item.ItemHash} (Instancia: {item.ItemInstanceId?.ToString() ?? "N/A"})");
                                    }
                                }
                            }
                             // Items en el inventario del personaje
                            if (destinyProfileResponse.CharacterInventories?.Data?.TryGetValue(firstCharacterId, out var charInventory) == true && charInventory.Items != null)
                            {
                                Console.WriteLine($"  Items en el inventario del personaje: {charInventory.Items.Count}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("[Main] No se pudieron obtener los datos del perfil de Destiny.");
                    }
                }
                else
                {
                     Console.WriteLine("[Main] No se pudo determinar un perfil de Destiny para cargar el inventario.");
                }
            }
            else
            {
                Console.WriteLine("[Main] No se encontraron perfiles de Destiny 2 vinculados.");
            }
        }
        else
        {
            Console.WriteLine("[Main] No se pudo obtener la información del perfil de Bungie.net.");
        }

        Console.WriteLine("[Main] Fin de la demostración. Presiona cualquier tecla para salir.");
        Console.ReadKey();
    }
}
