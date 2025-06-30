using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    public record InventoryItem
    {
        [JsonPropertyName("itemHash")]
        public uint ItemHash { get; init; }

        [JsonPropertyName("itemInstanceId")]
        public string? ItemInstanceId { get; init; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; init; }

        [JsonPropertyName("bindStatus")]
        public int BindStatus { get; init; } // Enum: BungieMembershipType

        [JsonPropertyName("location")]
        public int Location { get; init; } // Enum: ItemLocation

        [JsonPropertyName("bucketHash")]
        public uint BucketHash { get; init; }

        [JsonPropertyName("transferStatus")]
        public int TransferStatus { get; init; } // Enum: TransferStatuses

        [JsonPropertyName("lockable")]
        public bool Lockable { get; init; }

        [JsonPropertyName("state")]
        public int State { get; init; } // Enum: ItemState

        [JsonPropertyName("dismantlePermission")]
        public int DismantlePermission { get; init; } // Enum: DismantlePermission

        [JsonPropertyName("isWrapper")]
        public bool? IsWrapper { get; init; }

        [JsonPropertyName("tooltipNotificationIndexes")]
        public List<int>? TooltipNotificationIndexes { get; init; }

        // Para facilitar el acceso a los componentes instanciados, podríamos añadir propiedades aquí
        // que se llenarían después desde DestinyItemComponents.
        // Por ejemplo:
        // public ItemInstanceComponent? InstanceData { get; set; }
        // public ItemStatsComponent? StatsData { get; set; }
        // public ItemSocketsComponent? SocketsData { get; set; }
        // public ItemPerksComponent? PerksData { get; set; }
    }
}
