// guardian-definitivo/GuardianUI/Converters/ImageSourceConverter.cs
using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform; // For IAssetLoader

namespace GuardianUI.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string url && !string.IsNullOrEmpty(url))
            {
                try
                {
                    if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    {
                        // URL absoluta (ej. https://www.bungie.net/...)
                        // Avalonia puede cargar esto directamente si es accesible.
                        return new Bitmap(url); // Esto puede necesitar async/await o un placeholder mientras carga.
                                                // Para una implementación robusta, se usaría una librería de carga de imágenes como FFImageLoading.Avalonia o similar.
                                                // O se cargaría en el ViewModel y se expondría un Bitmap ya cargado.
                    }
                    else if (url.StartsWith("/")) // Path relativo a Assets
                    {
                        // Path local relativo a los Assets del proyecto
                        // Asegúrate que la imagen esté como AvaloniaResource en el .csproj
                        // Ejemplo: <AvaloniaResource Include="Assets\**" />
                        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                        if (assets != null)
                        {
                             // Construir URI de assets: avares://NombreDelEnsamblado/RutaAlAsset
                            // El nombre del ensamblado se puede obtener dinámicamente o hardcodear si se conoce.
                            // Assembly.GetExecutingAssembly().GetName().Name para el ensamblado actual.
                            // Para este proyecto UI, podría ser "GuardianUI".
                            string assemblyName = "GuardianUI"; // Asumir este nombre para el proyecto UI
                            if (url.StartsWith("/Assets/")) // Normalizar path si es necesario
                            {
                                return new Bitmap(assets.Open(new Uri($"avares://{assemblyName}{url}")));
                            }
                            return new Bitmap(assets.Open(new Uri($"avares://{assemblyName}/Assets{url}")));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar imagen desde '{url}': {ex.Message}");
                    // Retornar una imagen placeholder en caso de error
                    return GetPlaceholderBitmap();
                }
            }
            return GetPlaceholderBitmap(); // Placeholder por defecto
        }

        private Bitmap GetPlaceholderBitmap()
        {
            // Cargar un placeholder desde los assets de la aplicación
            try
            {
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                if (assets != null)
                {
                    // Asegúrate que este path sea correcto y el asset exista
                    return new Bitmap(assets.Open(new Uri("avares://GuardianUI/Assets/avalonia-logo.ico")));
                }
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"Error al cargar imagen placeholder: {ex.Message}");
            }
            return null!; // O un Bitmap vacío/transparente si la carga del placeholder también falla
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
