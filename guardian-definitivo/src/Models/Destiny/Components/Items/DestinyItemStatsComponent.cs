// guardian-definitivo/src/Models/Destiny/Components/Items/DestinyItemStatsComponent.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Stats; // For DestinyStat

namespace GuardianDefinitivo.Models.Destiny.Components.Items
{
    /// <summary>
    /// If you want the stats on an item's instanced data, get this component.
    /// These are stats like Attack, Defense etc...otherwise known as "Stats".
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Entities-Items-DestinyItemStatsComponent.html#schema_Destiny-Entities-Items-DestinyItemStatsComponent
    /// </summary>
    public class DestinyItemStatsComponent
    {
        /// <summary>
        /// If the item has stats that it provides (damage, defense, etc...), they will be given as a dictionary of Stat Hash Values to currently active stat levels.
        /// </summary>
        [JsonPropertyName("stats")]
        public Dictionary<uint, DestinyStat>? Stats { get; set; } // Key is statHash
    }
}
