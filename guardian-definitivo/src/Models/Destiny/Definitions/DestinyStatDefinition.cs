// guardian-definitivo/src/Models/Destiny/Definitions/DestinyStatDefinition.cs
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Definitions.Common; // For DestinyDisplayPropertiesDefinition

namespace GuardianDefinitivo.Models.Destiny.Definitions
{
    /// <summary>
    /// This represents a stat that's applied to a character or an item.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyStatDefinition.html#schema_Destiny-Definitions-DestinyStatDefinition
    /// </summary>
    public class DestinyStatDefinition : DestinyDefinition
    {
        [JsonPropertyName("displayProperties")]
        public DestinyDisplayPropertiesDefinition? DisplayProperties { get; set; }

        /// <summary>
        /// True if the stat is computed rather than being stored.
        /// </summary>
        [JsonPropertyName("aggregationType")]
        public int AggregationType { get; set; } // Enum: DestinyStatAggregationType

        /// <summary>
        /// True if the stat is computed rather than being stored.
        /// </summary>
        [JsonPropertyName("hasComputedBlock")]
        public bool HasComputedBlock { get; set; }

        /// <summary>
        /// The category of the stat, according to the game.
        /// </summary>
        [JsonPropertyName("statCategory")]
        public int StatCategory { get; set; } // Enum: DestinyStatCategory

        [JsonPropertyName("interpolate")]
        public bool Interpolate {get; set;}
    }
}
