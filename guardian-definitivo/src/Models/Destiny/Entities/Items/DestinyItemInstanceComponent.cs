// guardian-definitivo/src/Models/Destiny/Entities/Items/DestinyItemInstanceComponent.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Stats; // For DestinyStat
using GuardianDefinitivo.Models.Destiny.Misc; // For DestinyEnergyTypeDefinition

namespace GuardianDefinitivo.Models.Destiny.Entities.Items
{
    /// <summary>
    /// If an item is "instanced", this will contain information about the item's instance that doesn't fit neatly into other components.
    /// One might say this is the "essential" instance data for the item.
    /// Items are instanced if they require information or state that can vary.
    /// For instance, weapons are Instanced: they are given a unique identifier, uniquely generated stats, and can have their properties altered.
    /// Non-instanced items have none of these things: for instance, Glimmer has no instance data because my Glimmer is the same as your Glimmer.
    /// You can tell from an item's definition whether it will be instanced or not by looking at the DestinyInventoryItemDefinition's definition.itemTypeAndTier Properties block.
    /// If it has the Instanced flag set, it will be instanced.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Entities-Items-DestinyItemInstanceComponent.html#schema_Destiny-Entities-Items-DestinyItemInstanceComponent
    /// </summary>
    public class DestinyItemInstanceComponent
    {
        /// <summary>
        /// If the item has a damage type, this is the item's current damage type.
        /// </summary>
        [JsonPropertyName("damageType")]
        public int? DamageType { get; set; } // Enum: DamageType

        /// <summary>
        /// The current damage type's hash, so you can look up localized info about it.
        /// </summary>
        [JsonPropertyName("damageTypeHash")]
        public uint? DamageTypeHash { get; set; }

        /// <summary>
        /// The item's "primary" stat value. (Try Not To Use This)
        /// We attach EIGHT stats to items. Even if showing only one were acceptable (it's not),
        /// how could you determine which one was the primary? (Answer: Our servers know Negative Twenty-Seven courses of action.)
        /// </summary>
        [JsonPropertyName("primaryStat")]
        public DestinyStat? PrimaryStat { get; set; } // DestinyStat model needed

        /// <summary>
        /// The Item's Power Level. This is a snapshot of recent data and should not be used for actual calculations of item power.
        /// Prefer using characterStats and the item's definition if you need to determine actual item power.
        /// </summary>
        [JsonPropertyName("itemLevel")]
        public int ItemLevel { get; set; }

        /// <summary>
        /// The "Quality" of the item. (Rate Me) This is a snapshot of recent data.
        /// Prefer using the item's definition if you need to determine actual item quality.
        /// </summary>
        [JsonPropertyName("quality")]
        public int Quality { get; set; }

        /// <summary>
        /// Is the item currently equipped on the given character?
        /// </summary>
        [JsonPropertyName("isEquipped")]
        public bool IsEquipped { get; set; }

        /// <summary>
        /// If this is an equippable item, you can check it here. Otherwise, this will be null.
        /// </summary>
        [JsonPropertyName("canEquip")]
        public bool CanEquip { get; set; }

        /// <summary>
        /// If the item cannot be equipped until you reach a certain level, that level will be reflected here.
        /// </summary>
        [JsonPropertyName("equipRequiredLevel")]
        public int EquipRequiredLevel { get; set; }

        /// <summary>
        /// Sometimes, there are limitations to equipping that are represented by character-level flags called "unlock hashes".
        /// This is a list of those requirement hashes that are not met. Use these to look up the descriptions of requirements that you have not met.
        /// </summary>
        [JsonPropertyName("unlockHashesRequiredToEquip")]
        public List<uint>? UnlockHashesRequiredToEquip { get; set; }

        /// <summary>
        /// If the item cannot be equipped because of a character class restriction, that restriction will be reflected here.
        /// </summary>
        [JsonPropertyName("cannotEquipReason")]
        public int? CannotEquipReason { get; set; } // Enum: EquipFailureReason

        /// <summary>
        /// If populated, this item has a breaker type corresponding to the given value. See DestinyBreakerTypeDefinition for more details.
        /// </summary>
        [JsonPropertyName("breakerType")]
        public int? BreakerType { get; set; } // Enum: DestinyBreakerType

        /// <summary>
        /// If populated, this is the hash identifier for the item's breaker type. See DestinyBreakerTypeDefinition for more details.
        /// </summary>
        [JsonPropertyName("breakerTypeHash")]
        public uint? BreakerTypeHash { get; set; }

        /// <summary>
        /// If this item has available energy capacity, this is the current capacity that has been used.
        /// </summary>
        [JsonPropertyName("energy")]
        public DestinyItemInstanceEnergy? Energy { get; set; } // DestinyItemInstanceEnergy model needed
    }

    /// <summary>
    /// Represents the energy capacity of an item.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Entities-Items-DestinyItemInstanceEnergy.html#schema_Destiny-Entities-Items-DestinyItemInstanceEnergy
    /// </summary>
    public class DestinyItemInstanceEnergy
    {
        /// <summary>
        /// The type of energy for this item. Plugs that require this energy type can be inserted into the item.
        /// </summary>
        [JsonPropertyName("energyTypeHash")]
        public uint EnergyTypeHash { get; set; }

        /// <summary>
        /// This is the enum version of the energy type.
        /// </summary>
        [JsonPropertyName("energyType")]
        public int EnergyType { get; set; } // Enum: DestinyEnergyType

        /// <summary>
        /// The total capacity of energy that the item has.
        /// </summary>
        [JsonPropertyName("energyCapacity")]
        public int EnergyCapacity { get; set; }

        /// <summary>
        /// The amount of energy currently used by the item's plugs.
        /// </summary>
        [JsonPropertyName("energyUsed")]
        public int EnergyUsed { get; set; }

        /// <summary>
        /// The amount of energy still available for inserting new plugs.
        /// </summary>
        [JsonPropertyName("energyUnused")]
        public int EnergyUnused { get; set; }
    }
}
