using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    /// <summary>
    /// Representa el inventario de un personaje específico (ítems no equipados).
    /// </summary>
    public record CharacterInventoryComponent
    {
        [JsonPropertyName("items")]
        public List<InventoryItem>? Items { get; init; }
    }
}
