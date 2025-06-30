using Avalonia.Controls.Design;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GuardianUI.Models; // We'll define this soon
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GuardianUI.ViewModels;

public partial class InventoryViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<InventoryItemDisplayViewModel> _equippedItems = new();

    [ObservableProperty]
    private ObservableCollection<InventoryItemDisplayViewModel> _characterItems = new();

    [ObservableProperty]
    private ObservableCollection<InventoryItemDisplayViewModel> _vaultItems = new();

    [ObservableProperty]
    private string _statusMessage = "Cargando inventario...";

    // TODO: Inject a service here to fetch actual inventory data
    // private readonly IInventoryService _inventoryService;

    // public InventoryViewModel(IInventoryService inventoryService)
    // {
    // _inventoryService = inventoryService;
    // LoadInventoryCommand = new AsyncRelayCommand(ExecuteLoadInventoryAsync);
    // }

    public InventoryViewModel() // Constructor for now, potentially for design data or simple init
    {
        LoadInventoryCommand = new AsyncRelayCommand(ExecuteLoadInventoryAsync);

        // Load design-time data if using Avalonia designer
        if (Design.IsDesignMode)
        {
            LoadDesignTimeData();
        }
    }

    [RelayCommand]
    private async Task ExecuteLoadInventoryAsync()
    {
        StatusMessage = "Cargando inventario desde la API...";
        EquippedItems.Clear();
        CharacterItems.Clear();
        VaultItems.Clear();

        // Simulate API call delay
        await Task.Delay(1500);

        // TODO: Replace with actual data fetching logic using _inventoryService
        // For now, using placeholder/sample data loading logic
        LoadDesignTimeData(count: 5); // Load a few sample items as if from API

        if (!EquippedItems.Any() && !CharacterItems.Any() && !VaultItems.Any())
        {
            StatusMessage = "No se encontraron objetos en el inventario o la API no está disponible.";
        }
        else
        {
            StatusMessage = $"Inventario cargado. Equipado: {EquippedItems.Count}, Personaje: {CharacterItems.Count}, Bóveda: {VaultItems.Count}";
        }
    }

    private void LoadDesignTimeData(int count = 3)
    {
        // Equipped Items
        for (int i = 1; i <= count; i++)
        {
            EquippedItems.Add(new InventoryItemDisplayViewModel
            {
                InstanceId = $"equip{i}",
                Name = $"Arma Equipada Épica {i}",
                ItemTypeDisplayName = "Fusil Automático",
                TierTypeName = "Legendary",
                IconPath = "/Assets/icons/placeholder_weapon.png", // Ensure this path is valid for design time
                PowerLevel = 750 + i * 10,
                CanEquip = true,
                IsEquipped = true
            });
        }

        // Character Items (Inventory)
        for (int i = 1; i <= count + 2; i++)
        {
            CharacterItems.Add(new InventoryItemDisplayViewModel
            {
                InstanceId = $"char{i}",
                Name = $"Armadura Rara {i}",
                ItemTypeDisplayName = "Casco",
                TierTypeName = i % 2 == 0 ? "Rare" : "Uncommon",
                IconPath = "/Assets/icons/placeholder_armor.png",
                PowerLevel = 730 + i * 5,
                CanEquip = true,
                IsEquipped = false
            });
        }

        // Vault Items
        for (int i = 1; i <= count + 5; i++)
        {
            VaultItems.Add(new InventoryItemDisplayViewModel
            {
                InstanceId = $"vault{i}",
                Name = $"Exótico de Bóveda {i}",
                ItemTypeDisplayName = "Cañón de Mano",
                TierTypeName = "Exotic",
                IconPath = "/Assets/icons/placeholder_exotic.png",
                PowerLevel = 800 + i * 2,
                CanEquip = false, // Typically can't equip directly from vault view
                IsEquipped = false
            });
        }
        StatusMessage = "Datos de diseño cargados.";
    }
}

// This class would typically be in a Models folder/namespace
// For now, keeping it here for simplicity until we structure Models properly.
// Make sure this is compatible with the `x:DataType` in ItemCard.axaml
namespace GuardianUI.Models
{
    public partial class InventoryItemDisplayViewModel : ObservableObject // Using CommunityToolkit.Mvvm
    {
        [ObservableProperty]
        private string? _instanceId;

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _itemTypeDisplayName; // e.g., "Fusil Automático", "Casco"

        [ObservableProperty]
        private string? _tierTypeName; // e.g., "Exotic", "Legendary", "Rare"

        [ObservableProperty]
        private string? _iconPath; // Path to the item's icon (URL or local asset)

        [ObservableProperty]
        private int _powerLevel;

        [ObservableProperty]
        private bool _canEquip;

        [ObservableProperty]
        private bool _isEquipped;

        // Add other relevant properties:
        // Stats (List<StatViewModel>?)
        // Perks (List<PerkViewModel>?)
        // DamageTypeIcon (string path for Solar, Arc, Void, Stasis, Strand)
        // WatermarkIcon (string path for season/expansion icon)
        // Etc.
    }
}
