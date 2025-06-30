// guardian-definitivo/src/Models/Destiny/Perks/DestinyPerkReference.cs
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Perks
{
    /// <summary>
    /// The list of perks served by Socket Entries.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Perks-DestinyPerkReference.html#schema_Destiny-Perks-DestinyPerkReference
    /// </summary>
    public class DestinyPerkReference
    {
        /// <summary>
        /// The hash identifier for the perk, which can be used to look up DestinySandboxPerkDefinition if it exists.
        /// Be warned, perks frequently do not have user-viewable information. You should examine whether you actually found a name/description in the perk's definition before you show it to the user.
        /// </summary>
        [JsonPropertyName("perkHash")]
        public uint PerkHash { get; set; }

        /// <summary>
        /// The icon for the perk.
        /// </summary>
        [JsonPropertyName("iconPath")]
        public string? IconPath { get; set; }

        /// <summary>
        /// Whether this perk is currently active. (We may return perks that you have not actually activated yet: these ones will be inactive.)
        /// </summary>
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Some perks provide benefits that are specific to a particular Teammate or Player.
        /// This is the teammate or player hash identifier of that whom this perk applies to.
        /// </summary>
        [JsonPropertyName("visible")]
        public bool Visible { get; set; }
    }
}
