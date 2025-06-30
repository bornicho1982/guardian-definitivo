// guardian-definitivo/src/Data/BungieAuthConfig.cs
using System.Text.Json;

namespace GuardianDefinitivo.Data
{
    public class BungieAuthConfig
    {
        public string BungieApiKey { get; set; } = string.Empty;
        public string OAuthClientId { get; set; } = string.Empty;
        public string OAuthClientSecret { get; set; } = string.Empty; // Considerar manejo seguro para esto en producción

        public static BungieAuthConfig? Load(string configPath = "guardian-definitivo/src/AppConfig.json")
        {
            try
            {
                if (!File.Exists(configPath))
                {
                    Console.WriteLine($"Error: Archivo de configuración no encontrado en {configPath}");
                    return null;
                }

                var jsonString = File.ReadAllText(configPath);
                var configRoot = JsonSerializer.Deserialize<AppConfigRoot>(jsonString);
                return configRoot?.ToBungieAuthConfig();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar la configuración de Bungie: {ex.Message}");
                return null;
            }
        }
    }

    // Clase auxiliar para deserializar la raíz del AppConfig.json
    internal class AppConfigRoot
    {
        public string? BungieApiKey { get; set; }
        public string? OAuthClientId { get; set; }
        public string? OAuthClientSecret { get; set; }
        public string? DefaultLanguage { get; set; }
        public bool? EnableAnalytics { get; set; }
        public string? LastUsedProfile { get; set; }

        public BungieAuthConfig ToBungieAuthConfig()
        {
            return new BungieAuthConfig
            {
                BungieApiKey = this.BungieApiKey ?? string.Empty,
                OAuthClientId = this.OAuthClientId ?? string.Empty,
                OAuthClientSecret = this.OAuthClientSecret ?? string.Empty,
            };
        }
    }
}
