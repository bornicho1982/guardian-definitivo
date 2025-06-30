// guardian-definitivo/src/Models/Components/SingleComponentResponseOfT.cs
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Components
{
    /// <summary>
    /// A response wrapper for a single component.
    /// </summary>
    public class SingleComponentResponse<T> : ComponentResponse where T : class
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
