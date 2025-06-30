// guardian-definitivo/src/Models/Destiny/Components/Items/DestinyItemSocketsComponent.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Sockets; // For DestinyItemSocketState

namespace GuardianDefinitivo.Models.Destiny.Components.Items
{
    /// <summary>
    /// Instanced items can have sockets, which are slots that accept scaler-based plugs.
    /// These are outward-facing plugs that indicate power and stats that are owned by this item.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Entities-Items-DestinyItemSocketsComponent.html#schema_Destiny-Entities-Items-DestinyItemSocketsComponent
    /// </summary>
    public class DestinyItemSocketsComponent
    {
        /// <summary>
        /// The list of all sockets on the item, and their status.
        /// </summary>
        [JsonPropertyName("sockets")]
        public List<DestinyItemSocketState>? Sockets { get; set; }
    }
}
