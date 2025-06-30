// guardian-definitivo/src/Models/Destiny/Definitions/DestinyDefinition.cs
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Definitions
{
    /// <summary>
    /// Provides common properties for destiny definitions.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyDefinition.html#schema_Destiny-Definitions-DestinyDefinition
    /// </summary>
    public abstract class DestinyDefinition
    {
        /// <summary>
        /// The unique identifier for this entity. Guaranteed to be unique for the type of entity, but not globally.
        /// When entities refer to each other in Destiny content, it is this hash that they are referring to.
        /// </summary>
        [JsonPropertyName("hash")]
        public uint Hash { get; set; }

        /// <summary>
        /// The index of the entity as it was found in the investment tables.
        /// </summary>
        [JsonPropertyName("index")]
        public int Index { get; set; }

        /// <summary>
        /// If this is true, then there is an entity with this identifier/type combination, but BNet is not yet allowed to show it. Sorry!
        /// </summary>
        [JsonPropertyName("redacted")]
        public bool Redacted { get; set; }
    }
}
