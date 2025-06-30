// guardian-definitivo/src/Models/Destiny/Misc/DestinyColor.cs
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Misc
{
    /// <summary>
    /// Represents a color whose properties are numbers ranging from 0 to 255.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Misc-DestinyColor.html#schema_Destiny-Misc-DestinyColor
    /// </summary>
    public class DestinyColor
    {
        [JsonPropertyName("red")]
        public byte Red { get; set; }

        [JsonPropertyName("green")]
        public byte Green { get; set; }

        [JsonPropertyName("blue")]
        public byte Blue { get; set; }

        [JsonPropertyName("alpha")]
        public byte Alpha { get; set; }

        // Helper para convertir a un formato común si es necesario, ej. Hex
        public string ToHex() => $"#{Red:X2}{Green:X2}{Blue:X2}{Alpha:X2}";
    }
}
