using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    /// <summary>
    /// Representa el inventario del perfil (comúnmente, la bóveda).
    /// </summary>
    public record ProfileInventoryComponent
    {
        [JsonPropertyName("items")]
        public List<InventoryItem>? Items { get; init; }
    }
}
