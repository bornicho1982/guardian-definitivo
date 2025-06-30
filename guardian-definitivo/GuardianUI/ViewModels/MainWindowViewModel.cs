// guardian-definitivo/GuardianUI/ViewModels/MainWindowViewModel.cs
namespace GuardianUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase? _currentView;
        public ViewModelBase? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        // Comandos para cambiar de vista irían aquí
        // Ej: public ICommand NavigateToInventoryCommand { get; }

        public MainWindowViewModel()
        {
            // Vista inicial podría ser el inventario o un dashboard
            // CurrentView = new InventoryViewModel(); // Necesitaría InventoryViewModel
        }
    }
}
