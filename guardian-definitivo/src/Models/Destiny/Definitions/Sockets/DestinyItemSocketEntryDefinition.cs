// guardian-definitivo/src/Models/Destiny/Definitions/Sockets/DestinyItemSocketEntryDefinition.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Definitions.Sockets
{
    /// <summary>
    /// The static definition for item sockets. This will determine information like the type of socket,
    /// the appearance of the socket, and sometimes restrictions on what can be inserted in the socket.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyItemSocketEntryDefinition.html#schema_Destiny-Definitions-DestinyItemSocketEntryDefinition
    /// </summary>
    public class DestinyItemSocketEntryDefinition
    {
        /// <summary>
        /// All sockets have a type, and this is the hash identifier for this particular type. Use it to look up DestinySocketTypeDefinition.
        /// </summary>
        [JsonPropertyName("socketTypeHash")]
        public uint SocketTypeHash { get; set; }

        /// <summary>
        /// If a valid hash, this is the hash identifier for the DestinyInventoryItemDefinition representing the single item that can be inserted into this socket.
        /// Otherwise, this field is null.
        /// </summary>
        [JsonPropertyName("singleInitialItemHash")]
        public uint SingleInitialItemHash { get; set; } // Note: API says "if a valid hash", implies it can be 0 if not applicable.

        /// <summary>
        /// Identifies all items that can be inserted into this socket.
        /// </summary>
        [JsonPropertyName("reusablePlugItems")]
        public List<DestinyItemSocketEntryPlugItemDefinition>? ReusablePlugItems { get; set; }

        /// <summary>
        /// If this is true, then the socket is visible in the item's detail screen.
        /// </summary>
        [JsonPropertyName("plugSources")]
        public int PlugSources { get; set; } // Enum: SocketPlugSources

        /// <summary>
        /// If this is true, then the socket is visible in the item's detail screen.
        /// OBSOLETE: use plugSources instead.
        /// </summary>
        [JsonPropertyName("isVisible")]
        public bool IsVisible_OBSOLETE { get; set; }


        /// <summary>
        /// This field is an immutable list of plug item hashes that are valid for this socket. Only if displayProperties permits it will this socket allow any other plugs to be inserted.
        /// </summary>
        [JsonPropertyName("reusablePlugSetHash")]
        public uint? ReusablePlugSetHash { get; set; }

        /// <summary>
        /// This field is an immutable list of plug item hashes that are valid for this socket. Only if displayProperties permits it will this socket allow any other plugs to be inserted.
        /// </summary>
        [JsonPropertyName("randomizedPlugSetHash")]
        public uint? RandomizedPlugSetHash { get; set; }

        /// <summary>
        /// If true, then this socket is related to an intrinsic perk. See the plugSources property for the type of intrinsic perk.
        /// </summary>
        [JsonPropertyName("defaultVisible")]
        public bool DefaultVisible { get; set; }
    }

    /// <summary>
    /// The definition of a known, reusable plug that can be inserted into a socket.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyItemSocketEntryPlugItemDefinition.html#schema_Destiny-Definitions-DestinyItemSocketEntryPlugItemDefinition
    /// </summary>
    public class DestinyItemSocketEntryPlugItemDefinition
    {
        /// <summary>
        /// The hash identifier of a DestinyInventoryItemDefinition representing the plug that can be inserted.
        /// </summary>
        [JsonPropertyName("plugItemHash")]
        public uint PlugItemHash { get; set; }
    }

    /// <summary>
    /// Defines a socket that has a plug associated with it intrinsically.
    /// This is useful for situations where the weapon needs to have a visual plug/perk defined on it, but you don't want it to be socketable.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyItemIntrinsicSocketEntryDefinition.html#schema_Destiny-Definitions-DestinyItemIntrinsicSocketEntryDefinition
    /// </summary>
    public class DestinyItemIntrinsicSocketEntryDefinition
    {
        /// <summary>
        /// Indicates the plug that is intrinsically inserted into this socket.
        /// </summary>
        [JsonPropertyName("plugItemHash")]
        public uint PlugItemHash { get; set; }

        /// <summary>
        /// Indicates the type of this intrinsic socket.
        /// </summary>
        [JsonPropertyName("socketTypeHash")]
        public uint SocketTypeHash { get; set; }

        /// <summary>
        /// If true, then this socket is visible in the item's detail screen.
        /// </summary>
        [JsonPropertyName("defaultVisible")]
        public bool DefaultVisible { get; set; }
    }
}
