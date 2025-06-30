using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    /// <summary>
    /// Representa los ítems equipados por un personaje específico.
    /// </summary>
    public record CharacterEquipmentComponent
    {
        [JsonPropertyName("items")]
        public List<InventoryItem>? Items { get; init; }
    }
}
