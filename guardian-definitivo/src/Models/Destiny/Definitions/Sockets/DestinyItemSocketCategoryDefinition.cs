// guardian-definitivo/src/Models/Destiny/Definitions/Sockets/DestinyItemSocketCategoryDefinition.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Definitions.Sockets
{
    /// <summary>
    /// Sockets are grouped into categories in the UI. These categories have conceptual meaning:
    /// For instance, UI Category "Weapon Perks" are sockets that choose one perk in a columm.
    /// UI Category "Weapon Mods" are sockets that allow you to insert mods into a weapon.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyItemSocketCategoryDefinition.html#schema_Destiny-Definitions-DestinyItemSocketCategoryDefinition
    /// </summary>
    public class DestinyItemSocketCategoryDefinition
    {
        /// <summary>
        /// The hash for the DestinySocketCategoryDefinition.
        /// </summary>
        [JsonPropertyName("socketCategoryHash")]
        public uint SocketCategoryHash { get; set; }

        /// <summary>
        /// A list of socket indexes that are part of this category.
        /// </summary>
        [JsonPropertyName("socketIndexes")]
        public List<int>? SocketIndexes { get; set; }
    }
}
