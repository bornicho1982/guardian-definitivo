// guardian-definitivo/src/Data/ManifestService.cs
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GuardianDefinitivo.Models.Destiny.Config;
using GuardianDefinitivo.Models.Destiny.Definitions;
using Microsoft.Data.Sqlite; // Añadido para SQLite

namespace GuardianDefinitivo.Data
{
    public class ManifestService : IDisposable
    {
        private readonly BungieApiClient _apiClient;
        private readonly HttpClient _httpClient; // Para descargar el archivo del manifest
        private readonly string _preferredLanguage;
        private readonly string _cacheDirectory;
        private string _currentManifestVersion = string.Empty;
        private string _manifestDbPath = string.Empty;

        private const string ManifestVersionFileName = "manifest_version.txt";
        private const string ManifestDbFileName = "world_content.sqlite3";

        public ManifestService(BungieApiClient apiClient, HttpClient httpClient, string preferredLanguage = "es", string? cacheBasePath = null)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _preferredLanguage = preferredLanguage;

            if (string.IsNullOrEmpty(cacheBasePath))
            {
                // Usar un subdirectorio dentro del directorio de la aplicación
                cacheBasePath = Path.Combine(AppContext.BaseDirectory, "DataCache");
            }
            _cacheDirectory = Path.Combine(cacheBasePath, "DestinyManifest");
            Directory.CreateDirectory(_cacheDirectory); // Asegurar que el directorio exista

            _manifestDbPath = Path.Combine(_cacheDirectory, ManifestDbFileName);

            // Cargar la versión del manifest almacenada localmente, si existe
            LoadLocalManifestVersion();
        }

        private void LoadLocalManifestVersion()
        {
            string versionFilePath = Path.Combine(_cacheDirectory, ManifestVersionFileName);
            if (File.Exists(versionFilePath))
            {
                _currentManifestVersion = File.ReadAllText(versionFilePath).Trim();
                Console.WriteLine($"[ManifestService] Versión local del Manifest cargada: {_currentManifestVersion}");
            }
            else
            {
                Console.WriteLine($"[ManifestService] No se encontró archivo de versión local del Manifest.");
            }
        }

        private void SaveLocalManifestVersion(string version)
        {
            string versionFilePath = Path.Combine(_cacheDirectory, ManifestVersionFileName);
            File.WriteAllText(versionFilePath, version);
            _currentManifestVersion = version;
            Console.WriteLine($"[ManifestService] Nueva versión del Manifest guardada localmente: {version}");
        }

        public async Task InitializeAsync(bool forceUpdate = false)
        {
            Console.WriteLine("[ManifestService] Inicializando ManifestService...");
            DestinyManifest? manifestConfig = await _apiClient.GetDestinyManifestAsync();

            if (manifestConfig == null || manifestConfig.Version == null)
            {
                Console.WriteLine("[ManifestService] Error: No se pudo obtener la configuración del Manifest desde la API de Bungie.");
                if (File.Exists(_manifestDbPath))
                {
                    Console.WriteLine($"[ManifestService] Usando la base de datos del Manifest local existente: {_manifestDbPath}");
                }
                else
                {
                     Console.WriteLine("[ManifestService] Error: No hay base de datos local del Manifest disponible.");
                }
                return;
            }

            Console.WriteLine($"[ManifestService] Versión del Manifest Remoto: {manifestConfig.Version}. Versión Local: {_currentManifestVersion}");

            if (forceUpdate || manifestConfig.Version != _currentManifestVersion || !File.Exists(_manifestDbPath))
            {
                if (forceUpdate) Console.WriteLine("[ManifestService] Forzando actualización del Manifest.");
                else if (manifestConfig.Version != _currentManifestVersion) Console.WriteLine("[ManifestService] Nueva versión del Manifest detectada.");
                else if (!File.Exists(_manifestDbPath)) Console.WriteLine("[ManifestService] Base de datos local del Manifest no encontrada. Descargando...");

                string? worldContentPath = null;
                if (manifestConfig.MobileWorldContentPaths != null)
                {
                    if (manifestConfig.MobileWorldContentPaths.TryGetValue(_preferredLanguage, out var pathForPreferredLang))
                    {
                        worldContentPath = pathForPreferredLang;
                    }
                    else if (manifestConfig.MobileWorldContentPaths.TryGetValue("en", out var pathForEn)) // Fallback a inglés
                    {
                        worldContentPath = pathForEn;
                        Console.WriteLine($"[ManifestService] Path del Manifest para '{_preferredLanguage}' no encontrado, usando 'en' como fallback.");
                    }
                    else if (manifestConfig.MobileWorldContentPaths.Any()) // Fallback al primero disponible
                    {
                        worldContentPath = manifestConfig.MobileWorldContentPaths.First().Value;
                         Console.WriteLine($"[ManifestService] Path del Manifest para '{_preferredLanguage}' y 'en' no encontrados, usando '{manifestConfig.MobileWorldContentPaths.First().Key}'.");
                    }
                }

                if (string.IsNullOrEmpty(worldContentPath))
                {
                    Console.WriteLine("[ManifestService] Error: No se pudo determinar la URL del archivo de contenido del Manifest.");
                    return;
                }

                string fullUrl = $"https://www.bungie.net{worldContentPath}";
                Console.WriteLine($"[ManifestService] Descargando Manifest desde: {fullUrl}");

                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(fullUrl, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    string tempZipPath = Path.Combine(_cacheDirectory, "manifest_temp.zip");

                    Console.WriteLine($"[ManifestService] Descargando en: {tempZipPath}");
                    using (var fileStream = new FileStream(tempZipPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fileStream);
                    }
                    Console.WriteLine("[ManifestService] Descarga completada.");

                    Console.WriteLine($"[ManifestService] Descomprimiendo Manifest en: {_manifestDbPath}");
                    if (File.Exists(_manifestDbPath))
                    {
                        File.Delete(_manifestDbPath); // Eliminar la base de datos antigua si existe
                    }
                    ZipFile.ExtractToDirectory(tempZipPath, _cacheDirectory, true); // El true sobrescribe

                    // El archivo dentro del ZIP puede tener un nombre variable, pero suele ser el único .content o .sqlite3
                    // Necesitamos encontrarlo y renombrarlo/moverlo a _manifestDbPath
                    var extractedFiles = Directory.GetFiles(_cacheDirectory, "*.content");
                    if (!extractedFiles.Any()) extractedFiles = Directory.GetFiles(_cacheDirectory, "*.sqlite3");
                    if (!extractedFiles.Any()) extractedFiles = Directory.GetFiles(_cacheDirectory, "*.*");


                    if (extractedFiles.Any())
                    {
                        string extractedDb = extractedFiles.First(f => !f.EndsWith(".zip") && !f.EndsWith(".txt"));
                        if (File.Exists(extractedDb) && extractedDb != _manifestDbPath)
                        {
                             File.Move(extractedDb, _manifestDbPath, true); // Sobrescribir si ya existe por alguna razón
                             Console.WriteLine($"[ManifestService] Manifest descomprimido y movido a: {_manifestDbPath}");
                        } else if (extractedDb == _manifestDbPath) {
                             Console.WriteLine($"[ManifestService] Manifest descomprimido directamente como: {_manifestDbPath}");
                        }
                    }
                    else
                    {
                        throw new FileNotFoundException("No se encontró el archivo de base de datos dentro del ZIP del Manifest.");
                    }

                    File.Delete(tempZipPath); // Limpiar el ZIP temporal
                    Console.WriteLine("[ManifestService] Manifest descomprimido y listo.");
                    SaveLocalManifestVersion(manifestConfig.Version);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ManifestService] Error al descargar o procesar el Manifest: {ex.Message}");
                    // Si falla, y teníamos una versión antigua, podríamos decidir seguir usándola o fallar completamente.
                    // Por ahora, si falla, no tendremos manifest a menos que ya existiera uno.
                    if (!File.Exists(_manifestDbPath)) {
                         _currentManifestVersion = string.Empty; // Invalidar versión si la DB no existe
                    }
                    return;
                }
            }
            else
            {
                Console.WriteLine("[ManifestService] La versión local del Manifest está actualizada y el archivo existe.");
            }

            if (File.Exists(_manifestDbPath))
            {
                Console.WriteLine($"[ManifestService] Usando base de datos del Manifest: {_manifestDbPath}");
                // Aquí se abriría la conexión a SQLite para futuras consultas
            }
            else
            {
                 Console.WriteLine($"[ManifestService] Error: La base de datos del Manifest no existe en la ruta esperada: {_manifestDbPath} después de la inicialización.");
            }
        }

        // --- Métodos de Consulta al Manifest ---

        private SqliteConnection GetOpenConnection()
        {
            if (!File.Exists(_manifestDbPath))
            {
                // Podríamos intentar inicializar/descargar aquí si no existe,
                // pero InitializeAsync debería haberse llamado primero.
                throw new FileNotFoundException($"La base de datos del Manifest no se encuentra en la ruta esperada: {_manifestDbPath}. Asegúrate de llamar a InitializeAsync primero.");
            }
            var connection = new SqliteConnection($"Data Source={_manifestDbPath}");
            connection.Open();
            return connection;
        }

        private async Task<T?> GetDefinitionAsync<T>(uint hash, string tableName) where T : DestinyDefinition
        {
            if (string.IsNullOrEmpty(_manifestDbPath) || !File.Exists(_manifestDbPath))
            {
                Console.WriteLine($"[ManifestService] Error: La ruta de la BD del Manifest no está configurada o el archivo no existe ('{_manifestDbPath}').");
                return null;
            }

            // Los hashes en la API son uint. En la BD SQLite, la columna 'id' es INTEGER.
            // Si el hash es > int.MaxValue (2,147,483,647), se almacena como un negativo en SQLite (int).
            // Necesitamos convertir el uint hash a un int que SQLite pueda entender para la búsqueda.
            // `id = CAST({hash} AS INTEGER)` o `id = {signedHash}`
            int signedHash = unchecked((int)hash);

            try
            {
                await using var connection = GetOpenConnection();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT json FROM {tableName} WHERE id = @id";
                command.Parameters.AddWithValue("@id", signedHash);

                await using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var jsonString = reader.GetString(0);
                    if (!string.IsNullOrEmpty(jsonString))
                    {
                        return JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ManifestService] Error consultando {tableName} para hash {hash} (signed: {signedHash}): {ex.Message}");
            }
            return null;
        }

        public async Task<DestinyInventoryItemDefinition?> GetInventoryItemDefinitionAsync(uint itemHash)
        {
            return await GetDefinitionAsync<DestinyInventoryItemDefinition>(itemHash, "DestinyInventoryItemDefinition");
        }

        public async Task<DestinyStatDefinition?> GetStatDefinitionAsync(uint statHash)
        {
            return await GetDefinitionAsync<DestinyStatDefinition>(statHash, "DestinyStatDefinition");
        }

        public async Task<DestinySandboxPerkDefinition?> GetPerkDefinitionAsync(uint perkHash)
        {
            // Nota: El nombre de la tabla para perks de armas/armaduras es DestinySandboxPerkDefinition
            return await GetDefinitionAsync<DestinySandboxPerkDefinition>(perkHash, "DestinySandboxPerkDefinition");
        }

        // Puedes añadir más métodos de consulta para otras definiciones aquí (e.g., DestinyObjectiveDefinition, DestinyActivityDefinition, etc.)

        public async Task<DestinyDamageTypeDefinition?> GetDamageTypeDefinitionAsync(uint damageTypeHash)
        {
            return await GetDefinitionAsync<DestinyDamageTypeDefinition>(damageTypeHash, "DestinyDamageTypeDefinition");
        }

        public async Task<DestinyClassDefinition?> GetClassDefinitionAsync(uint classHash)
        {
            return await GetDefinitionAsync<DestinyClassDefinition>(classHash, "DestinyClassDefinition");
        }

        public async Task<DestinyRaceDefinition?> GetRaceDefinitionAsync(uint raceHash)
        {
            return await GetDefinitionAsync<DestinyRaceDefinition>(raceHash, "DestinyRaceDefinition");
        }

        public async Task<DestinyGenderDefinition?> GetGenderDefinitionAsync(uint genderHash)
        {
            return await GetDefinitionAsync<DestinyGenderDefinition>(genderHash, "DestinyGenderDefinition");
        }

        // TODO: Consider adding methods for DestinySocketTypeDefinition, DestinyStatGroupDefinition, etc. if needed.

        public void Dispose()
        {
            // SqliteConnection se dispone en GetDefinitionAsync gracias al `await using`.
            // _httpClient no necesita ser dispuesto aquí si se pasa desde fuera y se reutiliza.
        }
    }
}
