// guardian-definitivo/src/Data/BungieAuthConfig.cs
using System.Text.Json;

namespace GuardianDefinitivo.Data
{
    public class BungieAuthConfig
    {
        public string BungieApiKey { get; set; } = string.Empty;
        public string OAuthClientId { get; set; } = string.Empty;
        public string OAuthClientSecret { get; private set; } = string.Empty; // Setter privado para controlar cómo se establece

        private const string AppSecretsPath = "guardian-definitivo/src/AppSecrets.json";
        private const string ClientSecretEnvVar = "GUARDIAN_DEFINITIVO_CLIENT_SECRET";

        public static BungieAuthConfig? Load(string configPath = "guardian-definitivo/src/AppConfig.json")
        {
            try
            {
                if (!File.Exists(configPath))
                {
                    Console.WriteLine($"[BungieAuthConfig] Error: Archivo de configuración principal no encontrado en {configPath}");
                    return null;
                }

                var jsonString = File.ReadAllText(configPath);
                var configRoot = JsonSerializer.Deserialize<AppConfigRoot>(jsonString);

                if (configRoot == null)
                {
                    Console.WriteLine($"[BungieAuthConfig] Error: No se pudo deserializar el archivo de configuración principal.");
                    return null;
                }

                var authConfig = new BungieAuthConfig
                {
                    BungieApiKey = configRoot.BungieApiKey ?? string.Empty,
                    OAuthClientId = configRoot.OAuthClientId ?? string.Empty
                    // OAuthClientSecret se cargará por separado
                };

                // 1. Intentar cargar desde variable de entorno
                string? secretFromEnv = Environment.GetEnvironmentVariable(ClientSecretEnvVar);
                if (!string.IsNullOrEmpty(secretFromEnv))
                {
                    authConfig.OAuthClientSecret = secretFromEnv;
                    Console.WriteLine($"[BungieAuthConfig] OAuthClientSecret cargado desde la variable de entorno '{ClientSecretEnvVar}'.");
                    return authConfig;
                }

                // 2. Intentar cargar desde AppSecrets.json
                if (File.Exists(AppSecretsPath))
                {
                    try
                    {
                        var secretsJsonString = File.ReadAllText(AppSecretsPath);
                        var secretsRoot = JsonSerializer.Deserialize<AppSecretsRoot>(secretsJsonString);
                        if (secretsRoot != null && !string.IsNullOrEmpty(secretsRoot.OAuthClientSecret))
                        {
                            authConfig.OAuthClientSecret = secretsRoot.OAuthClientSecret;
                            Console.WriteLine($"[BungieAuthConfig] OAuthClientSecret cargado desde '{AppSecretsPath}'.");
                            return authConfig;
                        }
                        else
                        {
                            Console.WriteLine($"[BungieAuthConfig] Advertencia: '{AppSecretsPath}' encontrado pero no contiene un OAuthClientSecret válido o está vacío.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[BungieAuthConfig] Advertencia: Error al leer o deserializar '{AppSecretsPath}'. {ex.Message}");
                    }
                }
                else
                {
                     Console.WriteLine($"[BungieAuthConfig] Info: Archivo '{AppSecretsPath}' no encontrado. Buscando en AppConfig.json (no recomendado para repos públicos).");
                }

                // 3. Usar el valor de AppConfig.json (placeholder o valor por defecto si no es público)
                // Es importante que el usuario sepa si este valor es el que se está usando.
                authConfig.OAuthClientSecret = configRoot.OAuthClientSecret ?? string.Empty;
                if (authConfig.OAuthClientSecret == "SECRET_CONFIGURADO_EXTERNAMENTE_VER_README" || string.IsNullOrEmpty(authConfig.OAuthClientSecret))
                {
                    Console.WriteLine($"[BungieAuthConfig] Advertencia: OAuthClientSecret no se encontró en variables de entorno ni en AppSecrets.json. " +
                                      $"Se utilizará el valor placeholder de AppConfig.json ('{authConfig.OAuthClientSecret}'). " +
                                      "La autenticación OAuth confidencial probablemente fallará. Por favor, configure el secret externamente.");
                }
                else
                {
                     Console.WriteLine($"[BungieAuthConfig] OAuthClientSecret cargado desde AppConfig.json. " +
                                       "Recuerda que esto no es seguro para repositorios públicos si contiene el secret real.");
                }

                return authConfig;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BungieAuthConfig] Error fatal al cargar la configuración de Bungie: {ex.Message}");
                return null;
            }
        }
    }

    // Clase auxiliar para deserializar la raíz del AppConfig.json
    internal class AppConfigRoot
    {
        public string? BungieApiKey { get; set; }
        public string? OAuthClientId { get; set; }
        public string? OAuthClientSecret { get; set; } // Se mantiene para la carga inicial
        public string? DefaultLanguage { get; set; }
        public bool? EnableAnalytics { get; set; }
        public string? LastUsedProfile { get; set; }
    }

    // Clase auxiliar para deserializar AppSecrets.json
    internal class AppSecretsRoot
    {
        public string? OAuthClientSecret { get; set; }
    }
}
