// guardian-definitivo/src/Models/Destiny/Definitions/DestinyAmmunitionTypeDefinition.cs
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Definitions.Common;

namespace GuardianDefinitivo.Models.Destiny.Definitions
{
    /// <summary>
    /// Defines an ammunition type.
    /// </summary>
    public class DestinyAmmunitionTypeDefinition : DestinyDefinition
    {
        [JsonPropertyName("displayProperties")]
        public DestinyDisplayPropertiesDefinition? DisplayProperties { get; set; }

        // The enum value for this ammunition type, if applicable.
        // Not explicitly in typical manifest structures for this def, but useful for mapping.
        // The actual enum value is typically derived from DestinyItemEquippingBlockDefinition.ammoType
        // public int EnumValue { get; set; } // Example: Primary = 1, Special = 2, Heavy = 3
    }
}
