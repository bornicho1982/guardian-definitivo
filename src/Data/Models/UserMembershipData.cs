using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    // Representa la parte "Response" de la llamada a GetMembershipsForCurrentUser
    public record UserMembershipData
    {
        [JsonPropertyName("destinyMemberships")]
        public List<DestinyMembership>? DestinyMemberships { get; init; }

        [JsonPropertyName("primaryMembershipId")]
        public string? PrimaryMembershipId { get; init; }

        [JsonPropertyName("bungieNetUser")]
        public BungieNetUser? BungieNetUser { get; init; }
    }

    public record BungieNetUser // Información general del usuario de Bungie.net
    {
        [JsonPropertyName("membershipId")]
        public string? MembershipId { get; init; } // Bungie.net membership ID

        [JsonPropertyName("uniqueName")]
        public string? UniqueName { get; init; } // Bungie Name (e.g., Player#1234)

        [JsonPropertyName("displayName")]
        public string? DisplayName { get; init; } // Might be legacy or platform-specific

        // Añade más propiedades de BungieNetUser según sea necesario
        // e.g., profilePicturePath, profileThemeName, etc.
    }

    // Clase contenedora para la respuesta completa de la API, incluyendo ErrorCode, etc.
    public record BungieApiResponse<T>
    {
        [JsonPropertyName("Response")]
        public T? Response { get; init; }

        [JsonPropertyName("ErrorCode")]
        public int ErrorCode { get; init; }

        [JsonPropertyName("ThrottleSeconds")]
        public int ThrottleSeconds { get; init; }

        [JsonPropertyName("ErrorStatus")]
        public string? ErrorStatus { get; init; }

        [JsonPropertyName("Message")]
        public string? Message { get; init; }

        [JsonPropertyName("MessageData")]
        public Dictionary<string, string>? MessageData { get; init; }
    }
}
