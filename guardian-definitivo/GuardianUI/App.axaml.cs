// guardian-definitivo/GuardianUI/App.axaml.cs
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GuardianUI.ViewModels; // Necesitarás este namespace
using GuardianUI.Views;     // Necesitarás este namespace

namespace GuardianUI
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
                desktop.MainWindow = new MainWindow
                {
                    // DataContext se establecerá en MainWindow.axaml.cs o aquí si es un ViewModel global
                    // DataContext = new MainWindowViewModel(),
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                // Para plataformas móviles/webassembly donde solo hay una vista principal
                // singleViewPlatform.MainView = new MainView
                // {
                //     DataContext = new MainViewModel()
                // };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
