using Avalonia.Controls;
// Potentially add using for ViewModel if commands are directly bound from here
// using GuardianUI.ViewModels;

namespace GuardianUI.Components;

public partial class Sidebar : UserControl
{
    public Sidebar()
    {
        InitializeComponent();
        // If the commands are on MainWindowViewModel, DataContext might be inherited
        // or explicitly set. For now, this component is self-contained in terms of XAML.
        // DataContext = new MainWindowViewModel(); // Example if it had its own VM or needed one
    }
}
