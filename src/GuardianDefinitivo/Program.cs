using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.IO; // Para File.ReadAllText en la carga de ApiKey
using System.Text.Json;
using System.Threading.Tasks;
using GuardianDefinitivo.Data;
using GuardianDefinitivo.Data.Models; // Para UserMembershipData y BungieSecrets
using GuardianDefinitivo.UI;       // Para MainWindow

namespace GuardianDefinitivo
{
    class Program
    {
        // Punto de entrada de la aplicación Avalonia.
        // Se recomienda que este método Main sea síncrono.
        [STAThread] // Necesario para aplicaciones de escritorio en algunas plataformas.
        public static void Main(string[] args)
        {
            // Ejecutamos la lógica asíncrona y esperamos que se complete.
            // Esto es un patrón común para inicializar cosas async antes de que la UI se muestre.
            try
            {
                AsyncMain(args).GetAwaiter().GetResult();
            }
            catch (ConfigurationException ex)
            {
                // Errores de configuración de BungieAuth (secrets.json)
                Console.WriteLine($"Error de Configuración Crítico: {ex.Message}");
                // Aquí podrías mostrar un MessageBox si tuvieras una forma antes de Avalonia.
                // Por ahora, la app de consola terminará.
                // En una app real, podrías tener una mini-ventana de error.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Inesperado Crítico durante la inicialización: {ex.Message}");
            }
        }

        // Lógica asíncrona principal
        static async Task AsyncMain(string[] args)
        {
            Console.WriteLine("👾 Guardián Definitivo iniciando...");

            // 1. Autenticación
            BungieAuth auth;
            string? accessToken;
            try
            {
                auth = new BungieAuth(); // Puede lanzar ConfigurationException si secrets.json falta/está mal
                accessToken = await auth.AutenticarAsync();
            }
            catch (ConfigurationException ex)
            {
                Console.WriteLine($"Error de configuración durante la autenticación: {ex.Message}");
                // Considerar mostrar un mensaje al usuario antes de salir si es posible
                return;
            }
            catch (Exception ex) // Otras excepciones durante AutenticarAsync
            {
                 Console.WriteLine($"Error inesperado durante la autenticación: {ex.Message}");
                 return;
            }


            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("❌ Error: No se pudo obtener el token de acceso de Bungie. La aplicación no puede continuar.");
                return;
            }
            Console.WriteLine("🔑 Token de acceso obtenido.");

            // 2. Obtener API Key (BungieAuth ya leyó secrets.json, evitemos leerlo de nuevo)
            // Por ahora, asumimos que BungieAuth podría exponerla o la leemos aquí una vez.
            // Idealmente, esto vendría de un servicio de configuración inyectado.
            string apiKey;
            try
            {
                // Re-leyendo secrets.json para obtener la API key. No es ideal, pero funciona por ahora.
                // BungieAuth ya lo valida, así que podemos ser un poco más directos.
                var secretsJson = File.ReadAllText("secrets.json");
                var secretsData = JsonSerializer.Deserialize<BungieSecrets>(secretsJson);
                apiKey = secretsData?.Bungie?.ApiKey ?? throw new ConfigurationException("ApiKey no pudo ser leída de secrets.json en Program.cs");
                 if (string.IsNullOrWhiteSpace(apiKey))
                    throw new ConfigurationException("Error: 'ApiKey' en secrets.json no puede estar vacío (leído en Program.cs).");
            }
            catch (ConfigurationException ex)
            {
                 Console.WriteLine($"Error de configuración al leer ApiKey: {ex.Message}");
                 return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer ApiKey de secrets.json: {ex.Message}");
                return;
            }


            // 3. Obtener datos del perfil
            var bungieClient = new BungieClient(apiKey, accessToken);
            UserMembershipData? userMembershipData = await bungieClient.GetMembershipDataAsync();

            if (userMembershipData == null)
            {
                Console.WriteLine("❌ Error: No se pudieron obtener los datos del perfil de Bungie. La aplicación no puede continuar con todos los datos.");
                // Podríamos decidir lanzar la UI con un mensaje de error o simplemente salir.
                // Por ahora, saldremos para mantenerlo simple.
                return;
            }
            Console.WriteLine("👤 Datos del perfil obtenidos.");

            // 4. Construir y lanzar la aplicación Avalonia
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(desktop =>
                {
                    // Asignamos la ventana principal con los datos obtenidos
                    desktop.MainWindow = new MainWindow(userMembershipData);
                }, args); // Pasamos los argumentos de Main a Avalonia
        }

        // Configura Avalonia AppBuilder.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}