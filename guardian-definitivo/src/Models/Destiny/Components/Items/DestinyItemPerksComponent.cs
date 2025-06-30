// guardian-definitivo/src/Models/Destiny/Components/Items/DestinyItemPerksComponent.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Perks; // For DestinyPerkReference

namespace GuardianDefinitivo.Models.Destiny.Components.Items
{
    /// <summary>
    /// Instanced items can have perks: benefits that the item bestows.
    /// These are related to DestinySandboxPerkDefinition, and sometimes - but not always - have human readable info.
    /// When they do, they are the icons and text that you see in an item's tooltip.
    /// Talent Grids, Sockets, and an item's common properties are all potentially sources of perks, but you can reach them all through this component property.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Entities-Items-DestinyItemPerksComponent.html#schema_Destiny-Entities-Items-DestinyItemPerksComponent
    /// </summary>
    public class DestinyItemPerksComponent
    {
        /// <summary>
        /// The list of perks currently active on the item.
        /// </summary>
        [JsonPropertyName("perks")]
        public List<DestinyPerkReference>? Perks { get; set; }
    }
}
