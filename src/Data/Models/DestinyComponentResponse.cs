using System.Collections.Generic; // Necesario para Dictionary
using System.Text.Json.Serialization; // Necesario para JsonPropertyName

namespace GuardianDefinitivo.Data.Models
{
    // Componente de respuesta para un solo conjunto de datos (ej. profile.data)
    public record SingleComponentResponse<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; init; }

        [JsonPropertyName("privacy")]
        public int Privacy { get; init; } // Enum: ComponentPrivacySetting

        [JsonPropertyName("disabled")]
        public bool? Disabled { get; init; } // Si el componente está deshabilitado
    }

    // Componente de respuesta para un diccionario de datos (ej. characterInventories.data)
    // La clave del diccionario suele ser un string (como characterId o itemInstanceId)
    public record DictionaryComponentResponse<TKey, TValue> where TKey : notnull
    {
        [JsonPropertyName("data")]
        public Dictionary<TKey, TValue>? Data { get; init; }

        [JsonPropertyName("privacy")]
        public int Privacy { get; init; } // Enum: ComponentPrivacySetting

        [JsonPropertyName("disabled")]
        public bool? Disabled { get; init; } // Si el componente está deshabilitado
    }
}
