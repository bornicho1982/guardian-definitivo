// guardian-definitivo/src/Models/Destiny/Definitions/Sockets/DestinyPlugSetDefinition.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Definitions.Common; // For DestinyDisplayPropertiesDefinition

namespace GuardianDefinitivo.Models.Destiny.Definitions.Sockets
{
    /// <summary>
    /// Sometimes, we have groups of reusable plugs that are defined identically and thus can be shared here.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-Sockets-DestinyPlugSetDefinition.html#schema_Destiny-Definitions-Sockets-DestinyPlugSetDefinition
    /// </summary>
    public class DestinyPlugSetDefinition : DestinyDefinition
    {
        [JsonPropertyName("displayProperties")]
        public DestinyDisplayPropertiesDefinition? DisplayProperties { get; set; } // Not officially in schema, but often present in practice.

        /// <summary>
        /// The list of plugs that are part of this plug set.
        /// </summary>
        [JsonPropertyName("reusablePlugItems")]
        public List<DestinyItemSocketEntryPlugItemDefinition>? ReusablePlugItems { get; set; } // Reuses model from DestinyItemSocketEntryDefinition

        /// <summary>
        /// If true, this plug set is used for sharing reusable plugs between characters.
        /// Otherwise, this plug set is used for sharing reusable plugs between items of the same Definition.
        /// </summary>
        [JsonPropertyName("isFakePlugSet")]
        public bool IsFakePlugSet { get; set; }
    }
}
