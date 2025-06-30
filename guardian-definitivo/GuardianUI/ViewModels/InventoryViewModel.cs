// guardian-definitivo/GuardianUI/ViewModels/InventoryViewModel.cs
using System.Collections.ObjectModel; // Para ObservableCollection
using System.Threading.Tasks;
using GuardianDefinitivo.Services; // Para AppServices
using GuardianDefinitivo.Models.Destiny.Entities.Items; // Para DestinyItemComponent
using GuardianDefinitivo.Models.Destiny.Definitions; // Para DestinyInventoryItemDefinition
// using GuardianDefinitivo.Core.Models; // Suponiendo que tendrás modelos de datos de inventario aquí

namespace GuardianUI.ViewModels
{
    // ViewModel para un solo item en la lista del inventario
    public class InventoryItemDisplayViewModel : ViewModelBase
    {
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _iconPath = string.Empty;
        public string IconPath // Debería ser una URL completa a bungie.net
        {
            get => _iconPath;
            set => SetProperty(ref _iconPath, value);
        }

        private string _tierTypeName = string.Empty;
        public string TierTypeName
        {
            get => _tierTypeName;
            set => SetProperty(ref _tierTypeName, value);
        }

        private int _powerLevel;
        public int PowerLevel
        {
            get => _powerLevel;
            set => SetProperty(ref _powerLevel, value);
        }

        // Podríamos añadir más propiedades como color de rareza, etc.
        public DestinyItemComponent ItemComponent { get; }
        public DestinyInventoryItemDefinition ItemDefinition { get; }
        public DestinyItemInstanceComponent? InstanceComponent { get; }


        public InventoryItemDisplayViewModel(
            DestinyItemComponent itemComponent,
            DestinyInventoryItemDefinition itemDefinition,
            DestinyItemInstanceComponent? instanceComponent)
        {
            ItemComponent = itemComponent;
            ItemDefinition = itemDefinition;
            InstanceComponent = instanceComponent;

            Name = itemDefinition.DisplayProperties?.Name ?? "Desconocido";
            if (!string.IsNullOrEmpty(itemDefinition.DisplayProperties?.Icon))
            {
                IconPath = $"https://www.bungie.net{itemDefinition.DisplayProperties.Icon}";
            }
            else
            {
                IconPath = "/Assets/avalonia-logo.ico"; // Placeholder si no hay icono
            }
            TierTypeName = itemDefinition.Inventory?.TierTypeName ?? "Común";
            PowerLevel = instanceComponent?.ItemLevel ?? 0;
        }
    }

    public class InventoryViewModel : ViewModelBase
    {
        private readonly AppServices? _appServices;
        public ObservableCollection<InventoryItemDisplayViewModel> EquippedItems { get; } = new();
        public ObservableCollection<InventoryItemDisplayViewModel> CharacterInventoryItems { get; } = new();
        public ObservableCollection<InventoryItemDisplayViewModel> VaultItems { get; } = new();

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        // Constructor para el diseñador de Avalonia (sin parámetros)
        public InventoryViewModel() : this(null)
        {
            if (Design.IsDesignMode) // Avalonia.Controls.Design.IsDesignMode
            {
                // Datos de ejemplo para el diseñador
                EquippedItems.Add(new InventoryItemDisplayViewModel(
                    new DestinyItemComponent { ItemHash = 123, Quantity = 1 },
                    new DestinyInventoryItemDefinition { DisplayProperties = new() { Name = "Arma Exótica de Diseño", Icon="/img/misc/missing_icon_d2.png" }, Inventory = new() { TierTypeName = "Exótico"} },
                    new DestinyItemInstanceComponent { ItemLevel = 1810 }
                ));
                 CharacterInventoryItems.Add(new InventoryItemDisplayViewModel(
                    new DestinyItemComponent { ItemHash = 456, Quantity = 1 },
                    new DestinyInventoryItemDefinition { DisplayProperties = new() { Name = "Armadura Legendaria Diseño", Icon="/img/misc/missing_icon_d2.png" }, Inventory = new() { TierTypeName = "Legendario"} },
                    new DestinyItemInstanceComponent { ItemLevel = 1800 }
                ));
            }
        }

        public InventoryViewModel(AppServices? appServices)
        {
            _appServices = appServices;
            // No cargamos datos aquí automáticamente para permitir que la vista lo controle o se haga bajo demanda.
        }

        public async Task LoadInventoryCommandAsync()
        {
            if (_appServices == null || _appServices.ManifestService == null || _appServices.CurrentDestinyProfileResponse == null || _appServices.PrimaryDestinyProfile == null)
            {
                StatusMessage = "Servicios no disponibles o perfil no cargado.";
                return;
            }

            IsLoading = true;
            StatusMessage = "Cargando inventario...";
            EquippedItems.Clear();
            CharacterInventoryItems.Clear();
            VaultItems.Clear();

            var profileResponse = _appServices.CurrentDestinyProfileResponse;
            var manifest = _appServices.ManifestService;

            // Personaje actual (asumimos el primero por ahora)
            if (profileResponse.Characters?.Data != null && profileResponse.Characters.Data.Any())
            {
                var characterId = _appServices.PrimaryDestinyProfile.membershipId; // Esto es el destinyMembershipId, necesitamos el characterId
                var firstCharacter = profileResponse.Characters.Data.Values.FirstOrDefault(c => c.MembershipId == _appServices.PrimaryDestinyProfile.membershipId);
                // La API devuelve un diccionario de CharacterId -> CharacterComponent. Necesitamos el CharacterId correcto.
                // Si tenemos múltiples personajes, necesitaremos una forma de seleccionar cuál mostrar.
                // Por ahora, tomamos el primer characterId del diccionario de personajes.
                if (profileResponse.Characters.Data.Any())
                {
                    characterId = profileResponse.Characters.Data.First().Key;

                    // Equipado
                    if (profileResponse.CharacterEquipment?.Data?.TryGetValue(characterId, out var equippedInv) == true)
                    {
                        foreach (var itemComponent in equippedInv.Items)
                        {
                            var itemDef = await manifest.GetInventoryItemDefinitionAsync(itemComponent.ItemHash);
                            if (itemDef != null)
                            {
                                profileResponse.ItemComponents?.Instances?.Data?.TryGetValue(itemComponent.ItemInstanceId ?? 0, out var instanceData);
                                EquippedItems.Add(new InventoryItemDisplayViewModel(itemComponent, itemDef, instanceData));
                            }
                        }
                    }
                    // Inventario del Personaje
                    if (profileResponse.CharacterInventories?.Data?.TryGetValue(characterId, out var charInv) == true)
                    {
                         foreach (var itemComponent in charInv.Items)
                        {
                            var itemDef = await manifest.GetInventoryItemDefinitionAsync(itemComponent.ItemHash);
                            if (itemDef != null)
                            {
                                profileResponse.ItemComponents?.Instances?.Data?.TryGetValue(itemComponent.ItemInstanceId ?? 0, out var instanceData);
                                CharacterInventoryItems.Add(new InventoryItemDisplayViewModel(itemComponent, itemDef, instanceData));
                            }
                        }
                    }
                }
            }

            // Bóveda
            if (profileResponse.ProfileInventory?.Data?.Items != null)
            {
                foreach (var itemComponent in profileResponse.ProfileInventory.Data.Items)
                {
                    var itemDef = await manifest.GetInventoryItemDefinitionAsync(itemComponent.ItemHash);
                    if (itemDef != null)
                    {
                        profileResponse.ItemComponents?.Instances?.Data?.TryGetValue(itemComponent.ItemInstanceId ?? 0, out var instanceData);
                        VaultItems.Add(new InventoryItemDisplayViewModel(itemComponent, itemDef, instanceData));
                    }
                }
            }

            StatusMessage = $"Inventario cargado. Equipado: {EquippedItems.Count}, Personaje: {CharacterInventoryItems.Count}, Bóveda: {VaultItems.Count}";
            IsLoading = false;
        }
    }
}
