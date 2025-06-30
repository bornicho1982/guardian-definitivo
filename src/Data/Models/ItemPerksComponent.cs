using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    public record ItemPerksComponent
    {
        // La API devuelve una lista de DestinyPerkReference
        [JsonPropertyName("perks")]
        public List<DestinyPerkReference>? Perks { get; init; }
    }
}
