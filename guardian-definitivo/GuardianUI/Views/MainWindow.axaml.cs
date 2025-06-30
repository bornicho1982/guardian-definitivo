// guardian-definitivo/GuardianUI/Views/MainWindow.axaml.cs
// Este archivo se corresponde con MainWindow.axaml
// La plantilla de Avalonia lo generará como MainWindow.axaml.cs
// Y el x:Class en MainWindow.axaml debería ser GuardianUI.Views.MainWindow
using Avalonia.Controls;

namespace GuardianUI.Views // Asegúrate que el namespace coincida con el x:Class
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Aquí puedes establecer el DataContext si no lo haces en App.axaml.cs
            // DataContext = new MainWindowViewModel();
        }
    }
}
