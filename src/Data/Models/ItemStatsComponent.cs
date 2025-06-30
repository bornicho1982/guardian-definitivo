using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    public record ItemStatsComponent
    {
        // La API devuelve un diccionario donde la clave es el hash del stat (uint)
        // y el valor es un objeto DestinyStat.
        [JsonPropertyName("stats")]
        public Dictionary<uint, DestinyStat>? Stats { get; init; }
    }
}
