// Archivo de entrada principal para la aplicación Guardián Definitivo (Consola)
using System;
using System.Linq;
using System.Threading.Tasks;
using GuardianDefinitivo.Services; // Para AppServices
using GuardianDefinitivo.Models.Destiny.Responses; // Para DestinyProfileResponse (aunque se accede via AppServices)
// using GuardianDefinitivo.UI.Helpers; // DisplayHelper se usa indirectamente a través de métodos de presentación

public class Program
{
    private static AppServices? appServices;

    public static async Task Main(string[] args)
    {
        Console.WriteLine("¡Bienvenido a Guardián Definitivo! (Versión de Consola)");

        appServices = new AppServices();
        await appServices.InitializeAsync();

        if (!appServices.IsInitialized || appServices.AuthConfig == null)
        {
            Console.WriteLine("[Main] Error fatal: AppServices no pudo inicializarse correctamente. Revisa la configuración.");
            Console.WriteLine("Presiona cualquier tecla para salir.");
            Console.ReadKey();
            return;
        }

        // Autenticación
        if (!await appServices.AuthenticateAsync())
        {
            Console.WriteLine("[Main] Autenticación fallida. Saliendo.");
            Console.WriteLine("Presiona cualquier tecla para salir.");
            Console.ReadKey();
            return;
        }

        // Cargar perfil de usuario
        if (!await appServices.LoadUserProfileAsync() || appServices.CurrentUserMembershipData == null)
        {
            Console.WriteLine("[Main] No se pudo cargar el perfil del usuario. Saliendo.");
            Console.WriteLine("Presiona cualquier tecla para salir.");
            Console.ReadKey();
            return;
        }

        DisplayUserProfile(appServices.CurrentUserMembershipData, appServices.PrimaryDestinyProfile);

        // Cargar inventario de Destiny
        if (appServices.PrimaryDestinyProfile == null)
        {
            Console.WriteLine("[Main] No hay perfil de Destiny seleccionado para cargar inventario. Saliendo.");
        }
        else
        {
            if (!await appServices.LoadDestinyInventoryAsync() || appServices.CurrentDestinyProfileResponse == null)
            {
                 Console.WriteLine("[Main] No se pudo cargar el inventario de Destiny.");
            }
            else
            {
                Console.WriteLine("[Main] ¡Inventario de Destiny cargado!");
                await DisplayDestinyInventoryAsync(appServices.CurrentDestinyProfileResponse, appServices.ManifestService);
            }
        }

        Console.WriteLine("\n[Main] Fin de la demostración. Presiona cualquier tecla para salir.");
        Console.ReadKey();
    }

    private static void DisplayUserProfile(GuardianDefinitivo.Models.UserMembershipData? membershipData, GuardianDefinitivo.Models.GroupV2.GroupUserInfoCard? primaryDestinyProfile)
    {
        if (membershipData == null || membershipData.bungieNetUser == null)
        {
            Console.WriteLine("[Display] No hay datos de membresía para mostrar.");
            return;
        }

        Console.WriteLine($"\n--- Perfil de Bungie.net: {membershipData.bungieNetUser.displayName} (ID: {membershipData.bungieNetUser.membershipId}) ---");
        if (membershipData.destinyMemberships != null && membershipData.destinyMemberships.Any())
        {
            Console.WriteLine("Perfiles de Destiny 2 vinculados:");
            foreach (var profile in membershipData.destinyMemberships)
            {
                string isPrimary = (primaryDestinyProfile != null && profile.membershipId == primaryDestinyProfile.membershipId) ? "[PRIMARIO/SELECCIONADO]" : "";
                Console.WriteLine($"  - Plataforma: {profile.membershipType} (ID: {profile.membershipId}) {isPrimary}");
                Console.WriteLine($"    Nombre Global: {profile.bungieGlobalDisplayName}#{profile.bungieGlobalDisplayNameCode}");
                Console.WriteLine($"    Nombre en Plataforma: {profile.displayName}");
            }
        }
        else
        {
            Console.WriteLine("No se encontraron perfiles de Destiny 2 vinculados.");
        }
    }

    private static async Task DisplayDestinyInventoryAsync(DestinyProfileResponse? profileResponse, ManifestService? manifestService)
    {
        if (profileResponse == null)
        {
            Console.WriteLine("[Display] No hay datos de perfil de Destiny para mostrar inventario.");
            return;
        }
        if (manifestService == null)
        {
            Console.WriteLine("[Display] ManifestService no disponible. No se pueden mostrar detalles de items.");
            // Podríamos mostrar hashes si quisiéramos un fallback aquí.
            return;
        }

        // Mostrar información de la bóveda
        if (profileResponse.ProfileInventory?.Data?.Items != null)
        {
            Console.WriteLine($"\n--- Bóveda ({profileResponse.ProfileInventory.Data.Items.Count} items) (mostrando hasta 3) ---");
            foreach (var itemComponent in profileResponse.ProfileInventory.Data.Items.Take(3))
            {
                await PrintItemDetailsAsync(itemComponent, profileResponse, manifestService);
            }
        }

        // Mostrar información del primer personaje
        if (profileResponse.Characters?.Data != null && profileResponse.Characters.Data.Any())
        {
            var firstCharacterEntry = profileResponse.Characters.Data.First();
            long characterId = firstCharacterEntry.Key;
            var characterData = firstCharacterEntry.Value;

            string className = await GetDefinitionNameAsync(manifestService.GetClassDefinitionAsync(characterData.ClassHash), characterData.ClassHash.ToString());
            string raceName = await GetDefinitionNameAsync(manifestService.GetRaceDefinitionAsync(characterData.RaceHash), characterData.RaceHash.ToString());
            string genderName = await GetDefinitionNameAsync(manifestService.GetGenderDefinitionAsync(characterData.GenderHash), characterData.GenderHash.ToString());

            Console.WriteLine($"\n--- Personaje: {className} {raceName} {genderName} (ID: {characterId}) ---");
            Console.WriteLine($"  Luz: {characterData.Light}, Nivel Base: {characterData.BaseCharacterLevel}");
            Console.WriteLine($"  Tiempo Jugado Total: {characterData.MinutesPlayedTotal} minutos");

            // Items Equipados
            if (profileResponse.CharacterEquipment?.Data?.TryGetValue(characterId, out var equippedInv) == true && equippedInv.Items != null)
            {
                Console.WriteLine("\n  --- Equipado ---");
                foreach (var itemComponent in equippedInv.Items)
                {
                    await PrintItemDetailsAsync(itemComponent, profileResponse, manifestService);
                }
            }

            // Inventario del Personaje
            if (profileResponse.CharacterInventories?.Data?.TryGetValue(characterId, out var charInv) == true && charInv.Items != null)
            {
                Console.WriteLine($"\n  --- Inventario del Personaje ({charInv.Items.Count} items) (mostrando hasta 5) ---");
                foreach (var itemComponent in charInv.Items.Take(5))
                {
                    await PrintItemDetailsAsync(itemComponent, profileResponse, manifestService);
                }
            }
        }
    }

    private static async Task PrintItemDetailsAsync(GuardianDefinitivo.Models.Destiny.Entities.Items.DestinyItemComponent itemComponent, DestinyProfileResponse profileResponse, ManifestService manifestService)
    {
        var itemDef = await manifestService.GetInventoryItemDefinitionAsync(itemComponent.ItemHash);
        if (itemDef == null)
        {
            Console.WriteLine($"    - ItemHash: {itemComponent.ItemHash} (Definición no encontrada en Manifest)");
            return;
        }

        GuardianDefinitivo.Models.Destiny.Entities.Items.DestinyItemInstanceComponent? instanceData = null;
        GuardianDefinitivo.Models.Destiny.Components.Items.DestinyItemStatsComponent? instanceStats = null;
        GuardianDefinitivo.Models.Destiny.Components.Items.DestinyItemPerksComponent? instancePerks = null;
        GuardianDefinitivo.Models.Destiny.Components.Items.DestinyItemSocketsComponent? instanceSockets = null;

        if (itemComponent.ItemInstanceId.HasValue && profileResponse.ItemComponents != null)
        {
            profileResponse.ItemComponents.Instances?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instanceData);
            profileResponse.ItemComponents.Stats?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instanceStats);
            profileResponse.ItemComponents.Perks?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instancePerks);
            profileResponse.ItemComponents.Sockets?.Data?.TryGetValue(itemComponent.ItemInstanceId.Value, out instanceSockets);
        }

        string formattedDetails = await GuardianDefinitivo.UI.Helpers.DisplayHelper.FormatItemDetailsAsync(
            manifestService, itemComponent, itemDef, instanceData, instanceStats, instancePerks, instanceSockets
        );
        Console.WriteLine(formattedDetails);
    }

    private static async Task<string> GetDefinitionNameAsync<T>(Task<T?> definitionTask, string fallbackHash) where T : GuardianDefinitivo.Models.Destiny.Definitions.DestinyDefinition
    {
        var definition = await definitionTask;
        return definition?.DisplayProperties?.Name ?? $"Hash: {fallbackHash}";
    }
}
