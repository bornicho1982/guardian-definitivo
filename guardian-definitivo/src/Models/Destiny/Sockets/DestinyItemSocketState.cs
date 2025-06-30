// guardian-definitivo/src/Models/Destiny/Sockets/DestinyItemSocketState.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Sockets
{
    /// <summary>
    /// The state of a specific item's socket.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Entities-Items-DestinyItemSocketState.html#schema_Destiny-Entities-Items-DestinyItemSocketState
    /// </summary>
    public class DestinyItemSocketState
    {
        /// <summary>
        /// The hash identifier of the plug currently inserted in the socket.
        /// (Deprecated: use plugHash instead)
        /// </summary>
        [JsonPropertyName("plugHash_OBSOLETE")] // Marked as obsolete in some community docs, but still in schema
                                                // Let's keep it for now, but prioritize plugHash if available
        public uint? ObsoletePlugHash { get; set; }


        /// <summary>
        /// The hash identifier of the plug currently inserted in the socket.
        /// If no plug is inserted, this will be null.
        /// </summary>
        [JsonPropertyName("plugHash")]
        public uint? PlugHash { get; set; }


        /// <summary>
        /// Even if a plug is inserted, it doesn't mean it's enabled.
        /// This flag indicates whether the plug is active and providing its benefits.
        /// </summary>
        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// A plug may theoretically provide benefits but not be visible - for instance,
        /// some older items use this flag to block you from changing the plug.
        /// </summary>
        [JsonPropertyName("isVisible")]
        public bool IsVisible { get; set; } // Note: Schema says "A plug may theoretically provide benefits but not be visible... if it is false, you won't be able to know what the plug is." This seems to imply it might control visibility of the plug's details.

        /// <summary>
        /// If a plug is inserted and enabled, this will be the hash of the plug that is active.
        /// This is redundant with plugHash and isEnabled, but convenient.
        /// (Deprecated: This appears to be an artifact of an earlier version of sockets. Use plugHash and isEnabled instead.)
        /// </summary>
        [JsonPropertyName("enableFailIndexes_OBSOLETE")] // Marked as obsolete in schema comments
        public List<int>? ObsoleteEnableFailIndexes { get; set; }

        /// <summary>
        /// If the item is a Plug, this is a list of plugs that this plug is allowing to be inserted.
        /// </summary>
        [JsonPropertyName("reusablePlugHashes_MAYBE_OBSOLETE")] // Schema has this as reusablePlugHashes, but DestinyItemReusablePlugsComponent exists.
                                                                // This specific field might be for a different context or older system.
                                                                // Let's include it but be wary.
        public List<uint>? ReusablePlugHashes { get; set; }

        /// <summary>
        /// If the plug cannot be inserted for some reason, this will have the indexes into the plug item definition's plug.enabledRules property, indicating the rules that failed.
        /// </summary>
        [JsonPropertyName("plugDisabledErrorIdentifier")]
        public string? PlugDisabledErrorIdentifier { get; set; }
    }
}
