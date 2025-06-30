using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    public record DestinyPerkReference
    {
        [JsonPropertyName("perkHash")]
        public uint PerkHash { get; init; }

        [JsonPropertyName("iconPath")]
        public string? IconPath { get; init; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; init; }

        [JsonPropertyName("visible")]
        public bool Visible { get; init; }
    }
}
