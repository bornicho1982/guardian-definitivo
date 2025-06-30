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
                        // Componentes de Item:
                        DestinyComponentType.ItemInstances,     // Datos de instancia (nivel de luz, tipo de daño de instancia, etc.)
                        DestinyComponentType.ItemSockets,       // Sockets equipados (mods)
                        DestinyComponentType.ItemPerks,         // Perks activos en el item (de la instancia)
                        DestinyComponentType.ItemStats,         // Stats de la instancia del item
                        DestinyComponentType.ItemReusablePlugs, // Para ver opciones de perks/mods si es necesario más adelante
                        DestinyComponentType.ItemPlugObjectives // Progreso de objetivos en plugs (catalizadores, etc.)
                        // No necesitamos ItemRenderData (303) ni ItemTalentGrids (306) para la visualización básica
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

                            string className = "Desconocida";
                            string raceName = "Desconocida";
                            string genderName = "Desconocido";

                            if (manifestService != null)
                            {
                                var classDef = await manifestService.GetClassDefinitionAsync(characterData.ClassHash);
                                className = classDef?.DisplayProperties?.Name ?? className;
                                var raceDef = await manifestService.GetRaceDefinitionAsync(characterData.RaceHash);
                                raceName = raceDef?.DisplayProperties?.Name ?? raceName;
                                var genderDef = await manifestService.GetGenderDefinitionAsync(characterData.GenderHash);
                                genderName = genderDef?.DisplayProperties?.Name ?? genderName;
                            }

                            Console.WriteLine($"[Main] --- Personaje Principal ({characterData.CharacterId}) ---");
                            Console.WriteLine($"  Clase: {className}, Raza: {raceName}, Género: {genderName}");
                            Console.WriteLine($"  Luz: {characterData.Light}, Nivel: {characterData.BaseCharacterLevel}");
                            Console.WriteLine($"  Tiempo jugado: {characterData.MinutesPlayedTotal} minutos");

                            // Items equipados
                            if (destinyProfileResponse.CharacterEquipment?.Data?.TryGetValue(firstCharacterId, out var equippedItems) == true && equippedItems.Items != null)
                            {
                                Console.WriteLine("  --- Items Equipados ---");
                                if (manifestService != null) // Asegurarse que el manifest está disponible
                                {
                                    foreach (var itemComponent in equippedItems.Items) // No limitaremos a 5 por ahora para la prueba
                                    {
                                        var itemDef = await manifestService.GetInventoryItemDefinitionAsync(itemComponent.ItemHash);
                                        if (itemDef == null)
                                        {
                                            Console.WriteLine($"    - ItemHash: {itemComponent.ItemHash} (Definición no encontrada)");
                                            continue;
                                        }

                                        Models.Destiny.Entities.Items.DestinyItemInstanceComponent? instanceData = null;
                                        Models.Destiny.Components.Items.DestinyItemStatsComponent? instanceStats = null;
                                        Models.Destiny.Components.Items.DestinyItemPerksComponent? instancePerks = null;
                                        Models.Destiny.Components.Items.DestinyItemSocketsComponent? instanceSockets = null;

                                        if (itemComponent.ItemInstanceId.HasValue && destinyProfileResponse.ItemComponents != null)
                                        {
                                            destinyProfileResponse.ItemComponents.Instances?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instanceData);
                                            destinyProfileResponse.ItemComponents.Stats?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instanceStats);
                                            destinyProfileResponse.ItemComponents.Perks?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instancePerks);
                                            destinyProfileResponse.ItemComponents.Sockets?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instanceSockets);
                                        }

                                        Console.WriteLine(await UI.Helpers.DisplayHelper.FormatItemDetailsAsync(
                                            manifestService, itemComponent, itemDef, instanceData, instanceStats, instancePerks, instanceSockets
                                        ));
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("    ManifestService no disponible para mostrar detalles de items.");
                                    foreach (var item in equippedItems.Items.Take(5)) // Fallback
                                    {
                                        Console.WriteLine($"    - ItemHash: {item.ItemHash} (Instancia: {item.ItemInstanceId?.ToString() ?? "N/A"})");
                                    }
                                }
                            }
                             // Items en el inventario del personaje (mostrar algunos)
                            if (destinyProfileResponse.CharacterInventories?.Data?.TryGetValue(firstCharacterId, out var charInventory) == true && charInventory.Items != null)
                            {
                                Console.WriteLine($"  --- Items en Inventario del Personaje ({charInventory.Items.Count} items) (mostrando hasta 3) ---");
                                if (manifestService != null)
                                {
                                    foreach (var itemComponent in charInventory.Items.Take(3))
                                    {
                                        var itemDef = await manifestService.GetInventoryItemDefinitionAsync(itemComponent.ItemHash);
                                        if (itemDef == null) { Console.WriteLine($"    - ItemHash: {itemComponent.ItemHash} (Definición no encontrada)"); continue; }

                                        Models.Destiny.Entities.Items.DestinyItemInstanceComponent? instanceData = null;
                                        Models.Destiny.Components.Items.DestinyItemStatsComponent? instanceStats = null;
                                        Models.Destiny.Components.Items.DestinyItemPerksComponent? instancePerks = null;
                                        Models.Destiny.Components.Items.DestinyItemSocketsComponent? instanceSockets = null;
                                        if (itemComponent.ItemInstanceId.HasValue && destinyProfileResponse.ItemComponents != null)
                                        {
                                            destinyProfileResponse.ItemComponents.Instances?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instanceData);
                                            destinyProfileResponse.ItemComponents.Stats?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instanceStats);
                                            destinyProfileResponse.ItemComponents.Perks?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instancePerks);
                                            destinyProfileResponse.ItemComponents.Sockets?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instanceSockets);
                                        }
                                        Console.WriteLine(await UI.Helpers.DisplayHelper.FormatItemDetailsAsync(
                                            manifestService, itemComponent, itemDef, instanceData, instanceStats, instancePerks, instanceSockets
                                        ));
                                    }
                                }
                            }

                            // Items en la Bóveda (mostrar algunos)
                            if (destinyProfileResponse.ProfileInventory?.Data?.Items != null && manifestService != null)
                            {
                                Console.WriteLine($"  --- Items en la Bóveda ({destinyProfileResponse.ProfileInventory.Data.Items.Count} items) (mostrando hasta 3) ---");
                                foreach (var itemComponent in destinyProfileResponse.ProfileInventory.Data.Items.Take(3))
                                {
                                    var itemDef = await manifestService.GetInventoryItemDefinitionAsync(itemComponent.ItemHash);
                                    if (itemDef == null) { Console.WriteLine($"    - ItemHash: {itemComponent.ItemHash} (Definición no encontrada)"); continue; }

                                    Models.Destiny.Entities.Items.DestinyItemInstanceComponent? instanceData = null;
                                    Models.Destiny.Components.Items.DestinyItemStatsComponent? instanceStats = null;
                                    Models.Destiny.Components.Items.DestinyItemPerksComponent? instancePerks = null;
                                    Models.Destiny.Components.Items.DestinyItemSocketsComponent? instanceSockets = null;
                                    if (itemComponent.ItemInstanceId.HasValue && destinyProfileResponse.ItemComponents != null)
                                    {
                                        destinyProfileResponse.ItemComponents.Instances?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instanceData);
                                        destinyProfileResponse.ItemComponents.Stats?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instanceStats);
                                        destinyProfileResponse.ItemComponents.Perks?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instancePerks);
                                        destinyProfileResponse.ItemComponents.Sockets?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instanceSockets);
                                    }
                                    Console.WriteLine(await UI.Helpers.DisplayHelper.FormatItemDetailsAsync(
                                        manifestService, itemComponent, itemDef, instanceData, instanceStats, instancePerks, instanceSockets
                                    ));
                                }
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
