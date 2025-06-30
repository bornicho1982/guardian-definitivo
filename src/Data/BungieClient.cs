using System.Net.Http.Headers;
using System.Text.Json;
using GuardianDefinitivo.Data.Models; // Modelos de datos que creamos
using System.Threading.Tasks; // Para Task
using System; // Para Console.WriteLine en caso de error (temporal)

namespace GuardianDefinitivo.Data
{
    public class BungieClient
    {
        private readonly HttpClient http = new();
        // Guardamos las opciones de JsonSerializer para reutilizarlas
        private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Es útil si el casing de la API no siempre coincide con el de los modelos C#
        };

        // apiKey no se usa directamente en las llamadas autenticadas con Bearer token,
        // pero es bueno tenerlo si otros endpoints lo requieren explícitamente.
        // private readonly string apiKey;

        public BungieClient(string apiKey, string accessToken)
        {
            // this.apiKey = apiKey; // Guardar si se prevé usar en otros endpoints

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            http.DefaultRequestHeaders.Add("X-API-Key", apiKey); // X-API-Key es necesario para todas las llamadas a la API de Bungie
        }

        public async Task<UserMembershipData?> GetMembershipDataAsync()
        {
            Console.WriteLine("📡 Obteniendo datos de membresía del usuario..."); // Mensaje actualizado

            string url = "https://www.bungie.net/Platform/User/GetMembershipsForCurrentUser/";

            try
            {
                var responseMessage = await http.GetAsync(url);
                var content = await responseMessage.Content.ReadAsStringAsync();

                if (!responseMessage.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ Error HTTP al obtener datos de membresía: {responseMessage.StatusCode} - {content}");
                    // Podríamos intentar deserializar el error si Bungie devuelve un BungieApiResponse con el error
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<BungieApiResponse<object>>(content, jsonSerializerOptions);
                        if (errorResponse != null)
                        {
                             Console.WriteLine($"❌ API Error: {errorResponse.ErrorStatus} - {errorResponse.Message}");
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"❌ Error al deserializar la respuesta de error JSON: {ex.Message}");
                    }
                    return null;
                }

                var apiResponse = JsonSerializer.Deserialize<BungieApiResponse<UserMembershipData>>(content, jsonSerializerOptions);

                if (apiResponse == null)
                {
                    Console.WriteLine("❌ Error: No se pudo deserializar la respuesta de la API (apiResponse es null).");
                    return null;
                }

                if (apiResponse.ErrorCode != 1) // 1 generalmente significa Éxito (PlatformErrorCodes.Success)
                {
                    Console.WriteLine($"❌ Error de la API de Bungie: {apiResponse.ErrorStatus} (ErrorCode: {apiResponse.ErrorCode}) - {apiResponse.Message}");
                    return null;
                }

                if (apiResponse.Response == null)
                {
                    Console.WriteLine("❌ Error: La respuesta de la API no contenía los datos de membresía esperados (Response es null).");
                    return null;
                }

                Console.WriteLine("✅ Datos de membresía obtenidos correctamente.");
                return apiResponse.Response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"❌ Excepción de HttpRequest al obtener datos de membresía: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"❌ Excepción de Json al deserializar datos de membresía: {ex.Message}");
                return null;
            }
            catch (Exception ex) // Captura general para errores inesperados
            {
                Console.WriteLine($"❌ Error inesperado al obtener datos de membresía: {ex.Message}");
                return null;
            }
        }
    }
}