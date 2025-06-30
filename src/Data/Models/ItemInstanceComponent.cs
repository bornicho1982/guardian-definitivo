using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    public record ItemInstanceComponent
    {
        [JsonPropertyName("damageType")]
        public int? DamageType { get; init; } // Enum: DamageType

        [JsonPropertyName("damageTypeHash")]
        public uint? DamageTypeHash { get; init; }

        [JsonPropertyName("primaryStat")]
        public DestinyStat? PrimaryStat { get; init; } // Referencia a DestinyStat

        [JsonPropertyName("itemLevel")]
        public int ItemLevel { get; init; }

        [JsonPropertyName("quality")]
        public int Quality { get; init; } // Nivel de obra maestra, etc.

        [JsonPropertyName("isEquipped")]
        public bool IsEquipped { get; init; }

        [JsonPropertyName("canEquip")]
        public bool CanEquip { get; init; }

        [JsonPropertyName("equipRequiredLevel")]
        public int EquipRequiredLevel { get; init; }

        [JsonPropertyName("unlockHashesRequiredToEquip")]
        public List<uint>? UnlockHashesRequiredToEquip { get; init; }

        [JsonPropertyName("cannotEquipReason")]
        public int? CannotEquipReason { get; init; } // Enum: EquipFailureReason
    }
}
