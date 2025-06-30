namespace GuardianDefinitivo
{
    using System.Text.Json;
    using GuardianDefinitivo.Data;

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("👾 Ghost IA activado... Sincronizando con la Luz de la Torre...");

            var auth = new BungieAuth();
            string? token = await auth.AutenticarAsync();

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("❌ Error: No se recibió token de Bungie.");
                return;
            }

            var secrets = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(
                File.ReadAllText("secrets.json")
            );
            string apiKey = secrets["Bungie"]["ApiKey"];

            var bungie = new BungieClient(apiKey, token);
            await bungie.MostrarPerfilAsync();
        }
    }
}