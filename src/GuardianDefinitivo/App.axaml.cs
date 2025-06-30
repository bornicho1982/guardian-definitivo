using Avalonia;
using Avalonia.Controls; // Necesario para Window
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
// Asegúrate que estos usings apunten a la ubicación correcta de tus clases
// using GuardianDefinitivo.UI;
// using GuardianDefinitivo.Data.Models;

namespace GuardianDefinitivo
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Program.cs se encargará de instanciar MainWindow con los datos necesarios
                // y luego podría llamar a un método aquí si es necesario, o simplemente
                // pasarlo a StartWithClassicDesktopLifetime.
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                // Esto es más para plataformas móviles o WebAssembly.
                // Por ejemplo: singleViewPlatform.MainView = new Views.MainView();
            }

            base.OnFrameworkInitializationCompleted();
        }

        // Este método puede ser llamado desde Program.cs para establecer la ventana principal
        // si MainWindow se crea con parámetros allí.
        public void StartMainWindow(Window window)
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = window;
            }
        }
    }
}
