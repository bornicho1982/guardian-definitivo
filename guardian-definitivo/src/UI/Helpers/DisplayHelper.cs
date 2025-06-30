// guardian-definitivo/src/UI/Helpers/DisplayHelper.cs
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuardianDefinitivo.Data; // For ManifestService
using GuardianDefinitivo.Models.Destiny.Definitions;
using GuardianDefinitivo.Models.Destiny.Entities.Items;
using GuardianDefinitivo.Models.Destiny.Components.Items; // For instance components

namespace GuardianDefinitivo.UI.Helpers
{
    public static class DisplayHelper
    {
        public static async Task<string> FormatItemDetailsAsync(
            ManifestService manifestService,
            DestinyItemComponent item,
            DestinyInventoryItemDefinition itemDef,
            DestinyItemInstanceComponent? instanceData,
            DestinyItemStatsComponent? instanceStats, // Stats from item instance
            DestinyItemPerksComponent? instancePerks, // Perks from item instance/sockets
            DestinyItemSocketsComponent? instanceSockets // Sockets from item instance
            // Potentially add more components like DestinyItemReusablePlugsComponent if needed
        )
        {
            if (itemDef.DisplayProperties == null)
            {
                return $"Item Hash: {item.ItemHash} (Sin propiedades de visualización)";
            }

            var sb = new StringBuilder();
            sb.AppendLine($"--- {itemDef.DisplayProperties.Name} ({itemDef.ItemTypeDisplayName}) ---");

            if (!string.IsNullOrEmpty(itemDef.DisplayProperties.Description))
            {
                sb.AppendLine($"  \"{itemDef.DisplayProperties.Description}\"");
            }

            if (itemDef.Inventory != null)
            {
                sb.AppendLine($"  Tier: {itemDef.Inventory.TierTypeName ?? "Desconocido"}");
            }

            if (instanceData != null)
            {
                sb.AppendLine($"  Luz: {instanceData.ItemLevel}");
                if (instanceData.DamageTypeHash.HasValue)
                {
                    var damageTypeDef = await manifestService.GetDamageTypeDefinitionAsync(instanceData.DamageTypeHash.Value);
                    sb.AppendLine($"  Tipo de Daño (Instancia): {damageTypeDef?.DisplayProperties?.Name ?? instanceData.DamageTypeHash.Value.ToString()}");
                }
            }
            else if (itemDef.DefaultDamageTypeHash.HasValue) // Para items no instanciados con tipo de daño (ej. shaders con elemento)
            {
                 var damageTypeDef = await manifestService.GetDamageTypeDefinitionAsync(itemDef.DefaultDamageTypeHash.Value);
                 sb.AppendLine($"  Tipo de Daño (Defecto): {damageTypeDef?.DisplayProperties?.Name ?? itemDef.DefaultDamageTypeHash.Value.ToString()}");
            }


            if (itemDef.EquippingBlock != null && itemDef.EquippingBlock.AmmoType > 0)
            {
                // Para obtener el nombre del tipo de munición, necesitaríamos otra definición (DestinyAmmunitionTypeDefinition)
                // Por ahora, mostraremos el valor numérico del enum DestinyAmmunitionType.
                sb.AppendLine($"  Tipo de Munición: {(Models.Destiny.DestinyAmmunitionType)itemDef.EquippingBlock.AmmoType}");
            }

            // Mostrar Stats
            // Combinar stats base de la definición con stats de instancia
            var combinedStats = new Dictionary<uint, Models.Destiny.Stats.DestinyStat>();

            // Stats base de la definición del item (DestinyInventoryItemStatDefinition)
            if (itemDef.Stats?.Stats != null)
            {
                foreach (var statDefPair in itemDef.Stats.Stats)
                {
                    combinedStats[statDefPair.Key] = new Models.Destiny.Stats.DestinyStat { StatHash = statDefPair.Key, Value = statDefPair.Value.Value };
                }
            }
            // Stats de la instancia del item (DestinyStat)
            if (instanceStats?.Stats != null)
            {
                foreach (var instStatPair in instanceStats.Stats)
                {
                    // Sobrescribir o añadir, ya que las de instancia son las "finales" para ese roll
                    combinedStats[instStatPair.Key] = instStatPair.Value;
                }
            }

            if (combinedStats.Any())
            {
                sb.AppendLine("  Stats:");
                foreach (var statPair in combinedStats.OrderBy(s => GetStatDisplayOrder(s.Key))) // Ordenar por una lógica predefinida
                {
                    var statDef = await manifestService.GetStatDefinitionAsync(statPair.Key);
                    if (statDef?.DisplayProperties != null)
                    {
                        sb.AppendLine($"    - {statDef.DisplayProperties.Name}: {statPair.Value.Value}");
                    }
                }
            }

            // Mostrar Perks (Intrínsecos de la definición y de la instancia)
            var allPerkHashes = new HashSet<uint>();
            if (itemDef.Perks != null)
            {
                foreach(var perkEntry in itemDef.Perks) allPerkHashes.Add(perkEntry.PerkHash);
            }
            if (instancePerks?.Perks != null)
            {
                foreach(var perkRef in instancePerks.Perks) allPerkHashes.Add(perkRef.PerkHash);
            }
            // Podríamos también obtener perks de los sockets si están equipados, pero eso es más complejo por ahora.

            if (allPerkHashes.Any())
            {
                sb.AppendLine("  Perks Clave:");
                foreach (uint perkHash in allPerkHashes.Take(5)) // Mostrar los primeros N perks
                {
                    var perkDef = await manifestService.GetPerkDefinitionAsync(perkHash);
                    if (perkDef?.DisplayProperties != null && perkDef.IsDisplayable)
                    {
                        sb.AppendLine($"    * {perkDef.DisplayProperties.Name}");
                    }
                }
            }

            sb.AppendLine(); // Línea extra al final
            return sb.ToString();
        }

        // Helper para ordenar stats (ejemplo básico)
        private static int GetStatDisplayOrder(uint statHash)
        {
            // Estos son hashes comunes, se pueden encontrar en la documentación o explorando el manifest
            switch (statHash)
            {
                // Weapon Stats
                case 4043523819: return 1; // Impact
                case 1240592695: return 2; // Range
                case 155624089:  return 3; // Stability
                case 943549884:  return 4; // Handling
                case 4188031367: return 5; // Reload Speed
                case 2715839340: return 6; // Rounds Per Minute
                case 3555269338: return 7; // Magazine
                case 2523465841: return 8; // Aim Assistance
                case 2762071195: return 9; // Recoil Direction
                case 3022301683: return 10; // Zoom
                // Armor Stats (Mobility, Resilience, Recovery, Discipline, Intellect, Strength)
                case 2996146975: return 100; // Mobility
                case 3927672834: return 101; // Resilience
                case 1943323491: return 102; // Recovery
                case 1735777505: return 103; // Discipline
                case 144602215:  return 104; // Intellect
                case 4244567218: return 105; // Strength
                default: return 999; // Otros al final
            }
        }
    }
}

// Enum para DestinyAmmunitionType (basado en Destiny.DestinyAmmunitionType)
// Debería ir en un archivo de Enums si se usa más extensamente.
namespace GuardianDefinitivo.Models.Destiny
{
    public enum DestinyAmmunitionType
    {
        None = 0,
        Primary = 1,
        Special = 2,
        Heavy = 3,
        Unknown = 4
    }
}
