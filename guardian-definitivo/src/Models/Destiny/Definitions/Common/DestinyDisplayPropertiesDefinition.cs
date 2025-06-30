// guardian-definitivo/src/Models/Destiny/Definitions/Common/DestinyDisplayPropertiesDefinition.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Definitions.Common
{
    /// <summary>
    /// Many Destiny*Definition contracts - the "static" definition information for many game entities - represent the definition of an entity that has a Name, Description, and Icon.
    /// This is the common interface for all such entities. Note that DestinyObject is a base class for all items, and that many items also have display properties.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-Common-DestinyDisplayPropertiesDefinition.html#schema_Destiny-Definitions-Common-DestinyDisplayPropertiesDefinition
    /// </summary>
    public class DestinyDisplayPropertiesDefinition
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Note that "icon" is sometimes misleading, and should be interpreted as "backdrop" or "banner" often.
        /// For large icons, like those used by organizations and factions, this will be the large icon.
        /// For items, it will be the small version of the item's icon.
        /// </summary>
        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        /// <summary>
        /// If this item has a high-res icon (often, but not always, a larger version of the icon specified above), this will be the path to that icon.
        /// </summary>
        [JsonPropertyName("highResIcon")]
        public string? HighResIcon { get; set; }

        [JsonPropertyName("hasIcon")]
        public bool HasIcon { get; set; }

        // IconSequences, if needed, can be added later.
        // [JsonPropertyName("iconSequences")]
        // public List<DestinyIconSequenceDefinition> IconSequences { get; set; }
    }
}
