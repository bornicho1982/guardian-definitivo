// guardian-definitivo/src/Models/Destiny/Responses/DestinyItemComponentSetOfint64.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Components; // For DictionaryComponentResponseOfint64
using GuardianDefinitivo.Models.Destiny.Entities.Items;
// using GuardianDefinitivo.Models.Destiny.Components.Items; // Namespace for detailed item components

// Placeholders for component types not yet fully defined
// These will be actual classes in their respective files.
namespace GuardianDefinitivo.Models.Destiny.Components.Items
{
    public class DestinyItemObjectivesComponent { /* Placeholder */ }
    public class DestinyItemPerksComponent { /* Placeholder */ }
    public class DestinyItemRenderComponent { /* Placeholder */ }
    public class DestinyItemStatsComponent { /* Placeholder */ }
    public class DestinyItemSocketsComponent { /* Placeholder */ }
    public class DestinyItemTalentGridComponent { /* Placeholder */ }
    public class DestinyItemReusablePlugsComponent { /* Placeholder */ }
    public class DestinyItemPlugObjectivesComponent { /* Placeholder */ }
}


namespace GuardianDefinitivo.Models.Destiny.Responses
{
    /// <summary>
    /// The set of components belonging to an instanced item, keyed by the item's instance ID.
    /// Sourced from: https://bungie-net.github.io/multi/schema_DestinyItemComponentSetOfint64.html#schema_DestinyItemComponentSetOfint64
    /// </summary>
    public class DestinyItemComponentSetOfint64
    {
        [JsonPropertyName("instances")]
        public DictionaryComponentResponseOfint64<DestinyItemInstanceComponent>? Instances { get; set; }

        [JsonPropertyName("objectives")]
        public DictionaryComponentResponseOfint64<GuardianDefinitivo.Models.Destiny.Components.Items.DestinyItemObjectivesComponent>? Objectives { get; set; }

        [JsonPropertyName("perks")]
        public DictionaryComponentResponseOfint64<GuardianDefinitivo.Models.Destiny.Components.Items.DestinyItemPerksComponent>? Perks { get; set; }

        [JsonPropertyName("renderData")]
        public DictionaryComponentResponseOfint64<GuardianDefinitivo.Models.Destiny.Components.Items.DestinyItemRenderComponent>? RenderData { get; set; }

        [JsonPropertyName("stats")]
        public DictionaryComponentResponseOfint64<GuardianDefinitivo.Models.Destiny.Components.Items.DestinyItemStatsComponent>? Stats { get; set; }

        [JsonPropertyName("sockets")]
        public DictionaryComponentResponseOfint64<GuardianDefinitivo.Models.Destiny.Components.Items.DestinyItemSocketsComponent>? Sockets { get; set; }

        [JsonPropertyName("talentGrids")]
        public DictionaryComponentResponseOfint64<GuardianDefinitivo.Models.Destiny.Components.Items.DestinyItemTalentGridComponent>? TalentGrids { get; set; }

        [JsonPropertyName("plugStates")]
        public DictionaryComponentResponseOfint64<GuardianDefinitivo.Models.Destiny.Sockets.DestinyItemPlugComponent>? PlugStates { get; set; } // Re-check this type, schema says DictionaryComponentResponseOfuint32AndDestinyItemPlugComponent

        [JsonPropertyName("reusablePlugs")]
        public DictionaryComponentResponseOfint64<GuardianDefinitivo.Models.Destiny.Components.Items.DestinyItemReusablePlugsComponent>? ReusablePlugs { get; set; }

        [JsonPropertyName("plugObjectives")]
        public DictionaryComponentResponseOfint64<GuardianDefinitivo.Models.Destiny.Components.Items.DestinyItemPlugObjectivesComponent>? PlugObjectives { get; set; }
    }
}

// Placeholder for DestinyItemPlugComponent if not defined yet elsewhere
namespace GuardianDefinitivo.Models.Destiny.Sockets
{
    public class DestinyItemPlugComponent { /* Placeholder for now if not defined in its own file yet */ }
}
