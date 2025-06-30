using CommunityToolkit.Mvvm.ComponentModel;

namespace GuardianDefinitivo.Models.UI; // Corrected namespace

public partial class InventoryItemDisplayViewModel : ObservableObject
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
    private string? _iconPath; // Path to the item's icon (URL or local asset path for Avalonia)

    [ObservableProperty]
    private int _powerLevel;

    [ObservableProperty]
    private bool _canEquip;

    [ObservableProperty]
    private bool _isEquipped;

    // Add other relevant properties for UI display:
    // For example:
    // public ObservableCollection<StatViewModel> Stats { get; set; } = new();
    // public ObservableCollection<PerkViewModel> Perks { get; set; } = new();
    // public string? DamageTypeIconPath { get; set; }
    // public string? WatermarkIconPath { get; set; }
}
