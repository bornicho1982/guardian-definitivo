namespace GuardianUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _currentView;

    public ViewModelBase CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    public MainWindowViewModel()
    {
        // Initially, CurrentView could be a LoginViewModel or a DashboardViewModel
        // For now, let's assume we'll set it to InventoryViewModel as a test
        // This would typically be done after login or based on some other state.
        // _currentView = new InventoryViewModel(); // We'll create InventoryViewModel later
        _currentView = new PlaceholderViewModel("Welcome to Guardián Definitivo!"); // Default view
    }
}

// A simple placeholder ViewModel for initial display
public class PlaceholderViewModel : ViewModelBase
{
    public string Message { get; }
    public PlaceholderViewModel(string message)
    {
        Message = message;
    }
}

// You'll also need a corresponding View for PlaceholderViewModel if you want to display it
// e.g., PlaceholderView.axaml
// <UserControl ...> <TextBlock Text="{Binding Message}" HorizontalAlignment="Center" VerticalAlignment="Center"/> </UserControl>
