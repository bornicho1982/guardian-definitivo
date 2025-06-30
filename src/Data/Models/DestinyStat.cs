using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    public record DestinyStat
    {
        [JsonPropertyName("statHash")]
        public uint StatHash { get; init; }

        [JsonPropertyName("value")]
        public int Value { get; init; }

        // Podría incluirse "displayMaximum" si se usa para barras de UI, etc.
        // [JsonPropertyName("displayMaximum")]
        // public int? DisplayMaximum { get; init; }
    }
}
