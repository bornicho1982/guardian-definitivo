// guardian-definitivo/src/Models/Destiny/Definitions/Items/DestinyItemStatBlockDefinition.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Definitions.Items
{
    /// <summary>
    /// Information about the item's stats.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyItemStatBlockDefinition.html#schema_Destiny-Definitions-DestinyItemStatBlockDefinition
    /// </summary>
    public class DestinyItemStatBlockDefinition
    {
        /// <summary>
        /// If true, the game won't show the "primary" stat on this item when you inspect it.
        /// </summary>
        [JsonPropertyName("disablePrimaryStatDisplay")]
        public bool DisablePrimaryStatDisplay { get; set; }

        /// <summary>
        /// If the item's stats are meant to be modified by a DestinyStatGroupDefinition, this will be the identifier for that definition.
        /// </summary>
        [JsonPropertyName("statGroupHash")]
        public uint? StatGroupHash { get; set; }

        /// <summary>
        /// If you are looking for pre-calculated values for the stats on a weapon, this is where they are stored.
        /// Corresponds to the primary stats that you see in-game.
        /// </summary>
        [JsonPropertyName("stats")]
        public Dictionary<uint, DestinyInventoryItemStatDefinition>? Stats { get; set; } // Key is statHash

        /// <summary>
        /// A quick and lazy way to determine whether any stat other than the primary stat has a value.
        /// </summary>
        [JsonPropertyName("hasDisplayableStats")]
        public bool HasDisplayableStats { get; set; }

        /// <summary>
        /// This stat is determined primarily by the item's quality. This is the hash of the DestinyStatDefinition that holds the quality stat.
        /// </summary>
        [JsonPropertyName("primaryBaseStatHash")]
        public uint PrimaryBaseStatHash { get; set; }
    }

    /// <summary>
    /// Defines a specific stat that an Item Instance can have.
    /// This is the definition for stats that are intrinsic to an item, not those that are coming from plugs.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyInventoryItemStatDefinition.html#schema_Destiny-Definitions-DestinyInventoryItemStatDefinition
    /// </summary>
    public class DestinyInventoryItemStatDefinition
    {
        /// <summary>
        /// The hash for the DestinyStatDefinition representing this stat.
        /// </summary>
        [JsonPropertyName("statHash")]
        public uint StatHash { get; set; }

        /// <summary>
        /// This value represents the stat value assuming the minimum possible roll, zero masterwork, and no mods.
        /// </summary>
        [JsonPropertyName("value")]
        public int Value { get; set; }

        /// <summary>
        /// The minimum possible value for this stat.
        /// </summary>
        [JsonPropertyName("minimum")]
        public int Minimum { get; set; }

        /// <summary>
        /// The maximum possible value for this stat.
        /// </summary>
        [JsonPropertyName("maximum")]
        public int Maximum { get; set; }

        /// <summary>
        /// If the stat is on an item that can be masterworked, this is the maximum value the stat can achieve, including the masterwork benefit.
        /// </summary>
        [JsonPropertyName("displayMaximum")]
        public int? DisplayMaximum { get; set; }
    }
}
