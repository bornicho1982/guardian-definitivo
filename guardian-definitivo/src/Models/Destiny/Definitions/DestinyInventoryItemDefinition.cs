// guardian-definitivo/src/Models/Destiny/Definitions/DestinyInventoryItemDefinition.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Definitions.Common; // For DestinyDisplayPropertiesDefinition

namespace GuardianDefinitivo.Models.Destiny.Definitions
{
    /// <summary>
    /// So much of what you see in Destiny is actually an item. This is the definition of Items in Destiny.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyInventoryItemDefinition.html#schema_Destiny-Definitions-DestinyInventoryItemDefinition
    /// </summary>
    public class DestinyInventoryItemDefinition : DestinyDefinition
    {
        [JsonPropertyName("displayProperties")]
        public DestinyDisplayPropertiesDefinition? DisplayProperties { get; set; }

        /// <summary>
        /// Some items are actual visual elements: banners, emblems, shaders, ships, sparrows.
        /// These are the items that historically have had preview images rendered for them.
        /// </summary>
        [JsonPropertyName("secondaryIcon")]
        public string? SecondaryIcon { get; set; }

        /// <summary>
        /// A Secondary Icon property that is used when the item is in a vendor listing.
        /// </summary>
        [JsonPropertyName("secondaryOverlay")]
        public string? SecondaryOverlay { get; set; }

        /// <summary>
        /// A Secondary Icon property that is used when the item is displayed in the Character UI.
        /// </summary>
        [JsonPropertyName("secondarySpecial")]
        public string? SecondarySpecial { get; set; }

        [JsonPropertyName("backgroundColor")]
        public DestinyColorDefinition? BackgroundColor { get; set; } // DestinyColor model (needs to be defined or adapted)

        /// <summary>
        /// If the item has a screenshot, this is the path to that screenshot.
        /// </summary>
        [JsonPropertyName("screenshot")]
        public string? Screenshot { get; set; }

        /// <summary>
        /// The localized name of the item's type. This can be used to group items in the UI.
        /// </summary>
        [JsonPropertyName("itemTypeDisplayName")]
        public string? ItemTypeDisplayName { get; set; }

        /// <summary>
        /// A textual identifier for the item's UI "category". This is only available if the item actually still exists.
        /// </summary>
        [JsonPropertyName("uiItemDisplayStyle")]
        public string? UiItemDisplayStyle { get; set; }

        /// <summary>
        /// A string identifier that the game's UI uses to determine how the item is sorted in inventories.
        /// </summary>
        [JsonPropertyName("itemTypeAndTierDisplayName")]
        public string? ItemTypeAndTierDisplayName { get; set; }

        [JsonPropertyName("displaySource")]
        public string? DisplaySource { get; set; }

        [JsonPropertyName("tooltipStyle")]
        public string? TooltipStyle { get; set; }

        // Inventory block definition for stack size, tier, etc.
        [JsonPropertyName("inventory")]
        public DestinyItemInventoryBlockDefinition? Inventory { get; set; }

        // Other potentially useful fields to add later:
        // itemSubType, classType, equippable, defaultDamageType, investmentStats, perks, loreHash, etc.

        // Standard DestinyDefinition properties
        // These are inherited from the base class DestinyDefinition
        // [JsonPropertyName("hash")]
        // public uint Hash { get; set; }

        // [JsonPropertyName("index")]
        // public int Index { get; set; }

        // [JsonPropertyName("redacted")]
        // public bool Redacted { get; set; }
    }

    /// <summary>
    /// If the item has a color associated with it, this is the definition of that color.
    /// (Placeholder, actual DestinyColor might be different or need specific mapping)
    /// </summary>
    public class DestinyColorDefinition // Matches Destiny.Misc.DestinyColor structure
    {
        [JsonPropertyName("colorHash")]
        public uint ColorHash { get; set; } // Not in Destiny.Misc.DestinyColor, but in some defs.

        [JsonPropertyName("red")]
        public byte Red { get; set; }

        [JsonPropertyName("green")]
        public byte Green { get; set; }

        [JsonPropertyName("blue")]
        public byte Blue { get; set; }

        [JsonPropertyName("alpha")]
        public byte Alpha { get; set; }
    }

    /// <summary>
    /// If defined, this block defines how this item behaves in an inventory.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyItemInventoryBlockDefinition.html
    /// </summary>
    public class DestinyItemInventoryBlockDefinition
    {
        [JsonPropertyName("stackUniqueLabel")]
        public string? StackUniqueLabel { get; set; }

        [JsonPropertyName("maxStackSize")]
        public int MaxStackSize { get; set; }

        [JsonPropertyName("bucketTypeHash")]
        public uint BucketTypeHash { get; set; }

        [JsonPropertyName("recoveryBucketTypeHash")]
        public uint? RecoveryBucketTypeHash { get; set; }

        [JsonPropertyName("tierTypeHash")]
        public uint? TierTypeHash { get; set; }

        [JsonPropertyName("isInstanceItem")]
        public bool IsInstanceItem { get; set; }

        [JsonPropertyName("tierTypeName")]
        public string? TierTypeName { get; set; }

        [JsonPropertyName("tierType")]
        public int TierType { get; set; } // Enum: Destiny.TierType

        [JsonPropertyName("expirationTooltip")]
        public string? ExpirationTooltip { get; set; }

        [JsonPropertyName("expiredInActivityMessage")]
        public string? ExpiredInActivityMessage { get; set; }

        [JsonPropertyName("expiredInOrbitMessage")]
        public string? ExpiredInOrbitMessage { get; set; }

        [JsonPropertyName("suppressExpirationWhenObjectivesComplete")]
        public bool SuppressExpirationWhenObjectivesComplete { get; set; }
    }
}
