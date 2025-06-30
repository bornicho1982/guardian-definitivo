using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Data.Models
{
    /// <summary>
    /// Este componente contiene información sobre todos los ítems instanciados en el perfil.
    /// Las claves de los diccionarios son los itemInstanceId.
    /// </summary>
    public record DestinyItemComponents
    {
        [JsonPropertyName("instances")]
        public Dictionary<string, ItemInstanceComponent>? Instances { get; init; }

        [JsonPropertyName("perks")]
        public Dictionary<string, ItemPerksComponent>? Perks { get; init; }

        [JsonPropertyName("stats")]
        public Dictionary<string, ItemStatsComponent>? Stats { get; init; }

        [JsonPropertyName("sockets")]
        public Dictionary<string, ItemSocketsComponent>? Sockets { get; init; }

        // Otros componentes comunes que podrían añadirse si son necesarios:
        // [JsonPropertyName("renderData")]
        // public Dictionary<string, ItemRenderComponent>? RenderData { get; init; }

        // [JsonPropertyName("talentGrids")]
        // public Dictionary<string, ItemTalentGridComponent>? TalentGrids { get; init; }

        // [JsonPropertyName("plugStates")]
        // public Dictionary<string, ItemPlugStatesComponent>? PlugStates { get; init; } // Obsoleto, usar Sockets

        // [JsonPropertyName("objectives")]
        // public Dictionary<string, ItemObjectivesComponent>? Objectives { get; init; }
    }
}
