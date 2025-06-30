// guardian-definitivo/src/Models/Destiny/Definitions/DestinyDamageTypeDefinition.cs
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Definitions.Common;

namespace GuardianDefinitivo.Models.Destiny.Definitions
{
    /// <summary>
    /// All damage types that are possible in the game are defined here, along with localized info and icons as needed.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyDamageTypeDefinition.html#schema_Destiny-Definitions-DestinyDamageTypeDefinition
    /// </summary>
    public class DestinyDamageTypeDefinition : DestinyDefinition
    {
        [JsonPropertyName("displayProperties")]
        public DestinyDisplayPropertiesDefinition? DisplayProperties { get; set; }

        /// <summary>
        /// A color associated with the damage type.
        /// </summary>
        [JsonPropertyName("color")]
        public DestinyColorDefinition? Color { get; set; } // Using the existing DestinyColorDefinition from InventoryItemDefinition for now

        /// <summary>
        /// The icon that appears on television screens and interfaces.
        /// </summary>
        [JsonPropertyName("transparentIconPath")]
        public string? TransparentIconPath { get; set; }

        /// <summary>
        /// If TRUE, the game shows this damage type's icon. Otherwise, it doesn't.
        /// </summary>
        [JsonPropertyName("showIcon")]
        public bool ShowIcon { get; set; }

        /// <summary>
        /// We have an enumeration for damage types for quick reference. This is the current definition's damage type enum value.
        /// </summary>
        [JsonPropertyName("enumValue")]
        public int EnumValue { get; set; } // Enum: DamageType
    }
}
