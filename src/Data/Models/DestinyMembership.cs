using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    public record DestinyMembership
    {
        [JsonPropertyName("membershipType")]
        public int MembershipType { get; init; } // e.g., 1 for Xbox, 2 for PSN, 3 for Steam, etc.

        [JsonPropertyName("membershipId")]
        public string? MembershipId { get; init; } // Destiny-specific membership ID

        [JsonPropertyName("displayName")]
        public string? DisplayName { get; init; } // Bungie Name (e.g., Player#1234)

        [JsonPropertyName("bungieGlobalDisplayName")]
        public string? BungieGlobalDisplayName { get; init; } // Global display name, if different

        [JsonPropertyName("bungieGlobalDisplayNameCode")]
        public short? BungieGlobalDisplayNameCode { get; init; } // Numeric suffix for global name
    }
}
