using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    public record ItemSocketsComponent
    {
        // La API devuelve una lista de DestinyItemSocketState
        [JsonPropertyName("sockets")]
        public List<DestinyItemSocketState>? Sockets { get; init; }
    }
}
