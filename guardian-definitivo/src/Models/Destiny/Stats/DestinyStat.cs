// guardian-definitivo/src/Models/Destiny/Stats/DestinyStat.cs
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Stats
{
    /// <summary>
    /// Represents a stat on an item *or* character.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-DestinyStat.html#schema_Destiny-DestinyStat
    /// </summary>
    public class DestinyStat
    {
        /// <summary>
        /// The hash identifier for the Stat. Use it to look up the DestinyStatDefinition for static data about the stat.
        /// </summary>
        [JsonPropertyName("statHash")]
        public uint StatHash { get; set; }

        /// <summary>
        /// The current value of the Stat.
        /// </summary>
        [JsonPropertyName("value")]
        public int Value { get; set; }
    }
}
