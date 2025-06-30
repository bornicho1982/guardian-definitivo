// guardian-definitivo/src/Models/Destiny/Entities/Inventory/DestinyInventoryComponent.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Entities.Items; // For DestinyItemComponent

namespace GuardianDefinitivo.Models.Destiny.Entities.Inventory
{
    /// <summary>
    /// A list of items.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Entities-Inventory-DestinyInventoryComponent.html#schema_Destiny-Entities-Inventory-DestinyInventoryComponent
    /// </summary>
    public class DestinyInventoryComponent
    {
        /// <summary>
        /// The items in this inventory. If you care to bucket them, use the item's bucketHash.
        /// </summary>
        [JsonPropertyName("items")]
        public List<DestinyItemComponent>? Items { get; set; }
    }
}
