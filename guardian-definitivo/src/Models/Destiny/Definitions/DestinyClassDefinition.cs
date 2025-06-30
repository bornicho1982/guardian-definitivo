// guardian-definitivo/src/Models/Destiny/Definitions/DestinyClassDefinition.cs
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Definitions.Common;

namespace GuardianDefinitivo.Models.Destiny.Definitions
{
    /// <summary>
    /// Defines a Character Class in Destiny 2. These are types of characters you can play, like Titan, Hunter, and Warlock.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyClassDefinition.html#schema_Destiny-Definitions-DestinyClassDefinition
    /// </summary>
    public class DestinyClassDefinition : DestinyDefinition
    {
        [JsonPropertyName("displayProperties")]
        public DestinyDisplayPropertiesDefinition? DisplayProperties { get; set; }

        /// <summary>
        /// In Destiny 1, we added a convenience Enumeration for referring to classes.
        /// We've kept it, though mostly for posterity. This is the enum value for this definition.
        /// </summary>
        [JsonPropertyName("classType")]
        public int ClassType { get; set; } // Enum: DestinyClass (e.g. Titan = 0, Hunter = 1, Warlock = 2, Unknown = 3)

        [JsonPropertyName("genderedClassNames")]
        public Dictionary<string, string>? GenderedClassNames { get; set; } // Keyed by gender hash (string "0" or "1" etc.) or by gender enum name

        [JsonPropertyName("genderedClassNamesByGenderHash")]
        public Dictionary<uint, string>? GenderedClassNamesByGenderHash { get; set; } // Keyed by DestinyGenderDefinition hash
    }
}
