using System.Net.Http.Headers;
using System.Text.Json;

namespace GuardianDefinitivo.Data
{
    public class BungieClient
    {
        private readonly HttpClient http = new();
        private readonly string apiKey;
        private readonly string accessToken;

        public BungieClient(string apiKey, string token)
        {
            this.apiKey = apiKey;
            this.accessToken = token;

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            http.DefaultRequestHeaders.Add("X-API-Key", apiKey);
        }

        public async Task MostrarPerfilAsync()
        {
            Console.WriteLine("📡 Obteniendo perfil...");

            string url = "https://www.bungie.net/Platform/User/GetMembershipsForCurrentUser/";
            var res = await http.GetAsync(url);
            var content = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
            {
                Console.WriteLine("❌ Error: " + content);
                return;
            }

            using var json = JsonDocument.Parse(content);
            var root = json.RootElement;
            var name = root
                .GetProperty("Response")
                .GetProperty("destinyMemberships")[0]
                .GetProperty("displayName")
                .ToString();

            Console.WriteLine($"👤 Bienvenido, Guardián: {name}");
        }
    }
}