using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    public record DestinyItemSocketState
    {
        [JsonPropertyName("plugHash")]
        public uint? PlugHash { get; init; } // El hash del ítem (mod, perk, shader, etc.) insertado en el socket.

        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; init; }

        [JsonPropertyName("isVisible")]
        public bool IsVisible { get; init; }

        // Si el socket está deshabilitado, esto puede indicar por qué.
        // Se refiere a los índices en DestinySocketTypeDefinition.plugWhitelist.
        [JsonPropertyName("enableFailIndexes")]
        public List<int>? EnableFailIndexes { get; init; }

        // Podrían añadirse más propiedades si se necesitan, como:
        // reusablePlugHashes, plugObjectives, etc.
        // Por ahora, mantenemos los más comunes.
    }
}
