// guardian-definitivo/src/Models/Destiny/Definitions/Items/DestinyItemPerkEntryDefinition.cs
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Definitions.Items
{
    /// <summary>
    /// An item can have perks. This is a detailed definition of a perk on a specific item.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyItemPerkEntryDefinition.html#schema_Destiny-Definitions-DestinyItemPerkEntryDefinition
    /// </summary>
    public class DestinyItemPerkEntryDefinition
    {
        /// <summary>
        /// A hash that can be used to look up the DestinySandboxPerkDefinition if it exists.
        /// (NOTE: This is a DestinySandboxPerkDefinition, not a DestinyInventoryItemDefinition of a perk item. Perks are not items.)
        /// </summary>
        [JsonPropertyName("perkHash")]
        public uint PerkHash { get; set; }

        /// <summary>
        /// If this perk is not active, this is the string to show for why it's not providing its benefits.
        /// </summary>
        [JsonPropertyName("perkVisibility")]
        public int PerkVisibility { get; set; } // Enum: ItemPerkVisibility

        /// <summary>
        /// If this perk is related to a specific requirement, the requirement hash will be provided here.
        /// </summary>
        [JsonPropertyName("requirementDisplayString")]
        public string? RequirementDisplayString { get; set; }
    }
}
