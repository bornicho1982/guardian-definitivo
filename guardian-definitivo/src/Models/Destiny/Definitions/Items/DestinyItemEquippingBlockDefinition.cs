// guardian-definitivo/src/Models/Destiny/Definitions/Items/DestinyItemEquippingBlockDefinition.cs
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Definitions.Items
{
    /// <summary>
    /// Items that can be equipped define this block. It contains information about the slot where the item is equipped, and other customizability options.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyEquippingBlockDefinition.html#schema_Destiny-Definitions-DestinyEquippingBlockDefinition
    /// </summary>
    public class DestinyItemEquippingBlockDefinition
    {
        /// <summary>
        /// If the item is part of a gearset, this is a reference to that gearset item.
        /// </summary>
        [JsonPropertyName("gearsetItemHash")]
        public uint? GearsetItemHash { get; set; }

        /// <summary>
        /// If defined, this is the label used to check if the item has other items of the same type already equipped.
        /// For instance, when you aren't allowed to equip more than one Exotic Weapon, that's because all exotic weapons have identical uniqueLabels and the game checks the number of items with that label already equipped.
        /// </summary>
        [JsonPropertyName("uniqueLabel")]
        public string? UniqueLabel { get; set; }

        /// <summary>
        /// The hash of that unique label. Does not point to a specific definition.
        /// </summary>
        [JsonPropertyName("uniqueLabelHash")]
        public uint UniqueLabelHash { get; set; }

        /// <summary>
        /// An equipped item's slot type. This is an enum indicating what kind of equipment slot it goes into.
        /// </summary>
        [JsonPropertyName("equipmentSlotTypeHash")]
        public uint EquipmentSlotTypeHash { get; set; }

        /// <summary>
        /// The attributes of the item when equipped.
        /// </summary>
        [JsonPropertyName("attributes")]
        public int Attributes { get; set; } // Enum: EquippingItemBlockAttributes

        /// <summary>
        /// Ammo type used by a weapon is resolved by looking up the DestinyAmmoTypeDefinition for this hash.
        /// </summary>
        [JsonPropertyName("ammoType")]
        public int AmmoType { get; set; } // Enum: DestinyAmmunitionType

        /// <summary>
        /// These are custom attributes on the equippability of the item.
        /// For now, this can only be "equipmolesto" which only tells you if the item has stats that are affected by the overall stat rolls of every equipped item.
        /// </summary>
        [JsonPropertyName("displayStrings")]
        public List<string>? DisplayStrings { get; set; }
    }
}
