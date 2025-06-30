// guardian-definitivo/src/Models/Destiny/Definitions/Sockets/DestinyItemSocketBlockDefinition.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Definitions.Sockets
{
    /// <summary>
    /// If defined, the item has sockets. This block allows you to determine the Point Power granted by a socket entry.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyItemSocketBlockDefinition.html#schema_Destiny-Definitions-DestinyItemSocketBlockDefinition
    /// </summary>
    public class DestinyItemSocketBlockDefinition
    {
        /// <summary>
        /// This was supposed to be a string that would give per-item details about the socket block.
        /// It is now a stub andندہ should be ignored.
        /// </summary>
        [JsonPropertyName("detail")]
        public string? Detail { get; set; }

        /// <summary>
        /// Each non-intrinsic (or mutable) socket on an item is defined here. Check inside for more info.
        /// </summary>
        [JsonPropertyName("socketEntries")]
        public List<DestinyItemSocketEntryDefinition>? SocketEntries { get; set; }

        /// <summary>
        /// Each intrinsic (or immutable/permanent) socket on an item is defined here, along with the plug that is permanently affixed to the socket.
        /// </summary>
        [JsonPropertyName("intrinsicSockets")]
        public List<DestinyItemIntrinsicSocketEntryDefinition>? IntrinsicSockets { get; set; }


        /// <summary>
        /// A convenience property, that refers to the sockets in the "sockets" property, pre-grouped by category and ordered correctly according to BNet data.
        /// </summary>
        [JsonPropertyName("socketCategories")]
        public List<DestinyItemSocketCategoryDefinition>? SocketCategories { get; set; }
    }
}
