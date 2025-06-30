// guardian-definitivo/src/Models/Components/DictionaryComponentResponseOfint64AndT.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Components
{
    /// <summary>
    /// A response wrapper for a dictionary of components, keyed by a long (int64).
    /// Typically used for character-specific components.
    /// </summary>
    public class DictionaryComponentResponseOfint64<T> : ComponentResponse where T : class
    {
        [JsonPropertyName("data")]
        public Dictionary<long, T>? Data { get; set; }
    }
}
