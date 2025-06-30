// guardian-definitivo/src/Models/Components/ComponentResponse.cs
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny; // Para ComponentPrivacySetting

namespace GuardianDefinitivo.Models.Components
{
    /// <summary>
    /// The base class for all component responses.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Components-ComponentResponse.html#schema_Components-ComponentResponse
    /// </summary>
    public class ComponentResponse
    {
        [JsonPropertyName("privacy")]
        public ComponentPrivacySetting Privacy { get; set; }

        /// <summary>
        /// If true, this component is disabled.
        /// </summary>
        [JsonPropertyName("disabled")]
        public bool? Disabled { get; set; }
    }

    /// <summary>
    /// Sourced from: https://bungie-net.github.io/multi/schema_Components-ComponentPrivacySetting.html#schema_Components-ComponentPrivacySetting
    /// </summary>
    public enum ComponentPrivacySetting
    {
        None = 0,
        Public = 1,
        Private = 2
    }
}
