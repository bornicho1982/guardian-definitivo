// guardian-definitivo/src/Models/Destiny/Entities/Items/DestinyItemComponent.cs
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Quests; // For DestinyObjectiveProgress

namespace GuardianDefinitivo.Models.Destiny.Entities.Items
{
    /// <summary>
    /// The base item component, filled with properties that are generally useful to know in any item request or that don't feel worthwhile to put in their own component.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Entities-Items-DestinyItemComponent.html#schema_Destiny-Entities-Items-DestinyItemComponent
    /// </summary>
    public class DestinyItemComponent
    {
        /// <summary>
        /// The identifier for the item's definition, which is where most of the static properties for the item can be found.
        /// </summary>
        [JsonPropertyName("itemHash")]
        public uint ItemHash { get; set; }

        /// <summary>
        /// If the item is instanced, it will have an instance ID. Lack of an instance ID implies that the item has no instance data.
        /// </summary>
        [JsonPropertyName("itemInstanceId")]
        public long? ItemInstanceId { get; set; } // Represented as string in JSON

        /// <summary>
        /// The quantity of the item in this stack. Note that Instanced items cannot stack. If an instanced item, this value will always be 1 (zero issues reported).
        /// </summary>
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// If the item is bound to a location, it will be specified in this enum.
        /// </summary>
        [JsonPropertyName("bindStatus")]
        public int? BindStatus { get; set; } // Enum: ItemBindStatus

        /// <summary>
        /// An easy reference for where the item is located. Redundant if you got the item from an Inventory, but useful for from a Vendor an other alternative sources.
        /// </summary>
        [JsonPropertyName("location")]
        public int? Location { get; set; } // Enum: ItemLocation

        /// <summary>
        /// The hash identifier for the specific inventory bucket in which the item is located.
        /// </summary>
        [JsonPropertyName("bucketHash")]
        public uint BucketHash { get; set; }

        /// <summary>
        /// If there is a known error state that would cause this item to not be transferable, this Flags enum will indicate all of those error states. Otherwise, it will be 0.
        /// </summary>
        [JsonPropertyName("transferStatus")]
        public int? TransferStatus { get; set; } // Enum: TransferStatuses

        /// <summary>
        /// If the item can be locked, this will indicate that state.
        /// </summary>
        [JsonPropertyName("lockable")]
        public bool Lockable { get; set; }

        /// <summary>
        /// A flags enumeration indicating the states of the item. For example, the bitwise flags for IsTracked and IsMasterwork can be found here.
        /// </summary>
        [JsonPropertyName("state")]
        public int? State { get; set; } // Enum: ItemState

        /// <summary>
        /// If the item is a quest, this block will be non-null. In theory, instanced quest items will have instance-specific progress information, but don't hold your breath (you may always have to check the character's Progression).
        /// </summary>
        [JsonPropertyName("overrideStyleItemHash")]
        public uint? OverrideStyleItemHash { get; set; }

        /// <summary>
        /// If the item has a expiration date, this is when it will expire.
        /// </summary>
        [JsonPropertyName("expirationDate")]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// If this is populated, the item has a breaker type corresponding to the given value. See DestinyBreakerTypeDefinition for more details.
        /// </summary>
        [JsonPropertyName("breakerType")]
        public int? BreakerType {get; set;} // Enum: DestinyBreakerType

        /// <summary>
        /// If this is populated, the item has a breaker type corresponding to the given value. See DestinyBreakerTypeDefinition for more details.
        /// </summary>
        [JsonPropertyName("breakerTypeHash")]
        public uint? BreakerTypeHash {get; set;}


        /// <summary>
        /// If the item can be tracked, this will be the hash of the item to show as "tracking".
        /// </summary>
        [JsonPropertyName("tooltipNotificationIndexes")]
        public List<int>? TooltipNotificationIndexes { get; set; }

        /// <summary>
        /// The version number of this item, if it has one.
        /// </summary>
        [JsonPropertyName("versionNumber")]
        public int? VersionNumber { get; set; }

        /// <summary>
        /// If the item has a metric objective, this will be the hash of that objective.
        /// </summary>
        [JsonPropertyName("metricHash")]
        public uint? MetricHash { get; set; }

        /// <summary>
        /// If the item has a metric objective, this will be the progress of that objective.
        /// </summary>
        [JsonPropertyName("metricObjective")]
        public DestinyObjectiveProgress? MetricObjective { get; set; }
    }
}
