// guardian-definitivo/src/Models/Destiny/Definitions/DestinySandboxPerkDefinition.cs
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Definitions.Common; // For DestinyDisplayPropertiesDefinition

namespace GuardianDefinitivo.Models.Destiny.Definitions
{
    /// <summary>
    /// Perks are modifiers to a character or item that can be applied situationally.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinySandboxPerkDefinition.html#schema_Destiny-Definitions-DestinySandboxPerkDefinition
    /// </summary>
    public class DestinySandboxPerkDefinition : DestinyDefinition
    {
        [JsonPropertyName("displayProperties")]
        public DestinyDisplayPropertiesDefinition? DisplayProperties { get; set; }

        /// <summary>
        /// The string identifier for the perk.
        /// </summary>
        [JsonPropertyName("perkIdentifier")]
        public string? PerkIdentifier { get; set; }

        /// <summary>
        /// If true, this perk is currently active.
        /// </summary>
        [JsonPropertyName("isDisplayable")]
        public bool IsDisplayable { get; set; }

        /// <summary>
        /// If this perk grants a damage type to a weapon, the type will be defined here.
        /// </summary>
        [JsonPropertyName("damageType")]
        public int DamageType { get; set; } // Enum: DamageType

        /// <summary>
        /// The hash identifier for the damage type being granted, if it applies.
        /// </summary>
        [JsonPropertyName("damageTypeHash")]
        public uint? DamageTypeHash { get; set; }
    }
}
