// guardian-definitivo/src/Models/Destiny/Responses/DestinyProfileResponse.cs
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Components; // Para las clases ComponentResponse
// Placeholder for actual Destiny component types - will be replaced with specific classes
using GuardianDefinitivo.Models.Destiny.Entities.Profiles;
using GuardianDefinitivo.Models.Destiny.Entities.Characters;
using GuardianDefinitivo.Models.Destiny.Entities.Items;
using GuardianDefinitivo.Models.Destiny.Components.Profiles;
using GuardianDefinitivo.Models.Destiny.Components.Characters;
using GuardianDefinitivo.Models.Destiny.Components.Inventory;
using GuardianDefinitivo.Models.Destiny.Components.Kiosks;
using GuardianDefinitivo.Models.Destiny.Components.PlugSets;
using GuardianDefinitivo.Models.Destiny.Components.Presentation;
using GuardianDefinitivo.Models.Destiny.Components.Records;
using GuardianDefinitivo.Models.Destiny.Components.Collectibles;
using GuardianDefinitivo.Models.Destiny.Components.Metrics;
using GuardianDefinitivo.Models.Destiny.Components.StringVariables;
using GuardianDefinitivo.Models.Destiny.Components.Social;
using GuardianDefinitivo.Models.Destiny.Components.Loadouts;
using GuardianDefinitivo.Models.Destiny.Components.Craftables;


namespace GuardianDefinitivo.Models.Destiny.Responses
{
    /// <summary>
    /// The response for GetDestinyProfile, with components for character and item-level data.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Responses-DestinyProfileResponse.html#schema_Destiny-Responses-DestinyProfileResponse
    /// </summary>
    public class DestinyProfileResponse
    {
        [JsonPropertyName("responseMintedTimestamp")]
        public DateTime ResponseMintedTimestamp { get; set; }

        [JsonPropertyName("secondaryComponentsMintedTimestamp")]
        public DateTime? SecondaryComponentsMintedTimestamp { get; set; }

        // PROFILE SCOPED COMPONENTS
        [JsonPropertyName("vendorReceipts")]
        public SingleComponentResponse<DestinyVendorReceiptsComponent>? VendorReceipts { get; set; }

        [JsonPropertyName("profileInventory")]
        public SingleComponentResponse<DestinyInventoryComponent>? ProfileInventory { get; set; } // Vault

        [JsonPropertyName("profileCurrencies")]
        public SingleComponentResponse<DestinyInventoryComponent>? ProfileCurrencies { get; set; } // Actually DestinyCurrenciesComponent, but structure is similar for items list

        [JsonPropertyName("profile")]
        public SingleComponentResponse<DestinyProfileComponent>? Profile { get; set; }

        [JsonPropertyName("platformSilver")]
        public SingleComponentResponse<DestinyPlatformSilverComponent>? PlatformSilver { get; set; }

        [JsonPropertyName("profileKiosks")]
        public SingleComponentResponse<DestinyKiosksComponent>? ProfileKiosks { get; set; }

        [JsonPropertyName("profilePlugSets")]
        public SingleComponentResponse<DestinyPlugSetsComponent>? ProfilePlugSets { get; set; }

        [JsonPropertyName("profileProgression")]
        public SingleComponentResponse<DestinyProfileProgressionComponent>? ProfileProgression { get; set; }

        [JsonPropertyName("profilePresentationNodes")]
        public SingleComponentResponse<DestinyPresentationNodesComponent>? ProfilePresentationNodes { get; set; }

        [JsonPropertyName("profileRecords")]
        public SingleComponentResponse<DestinyProfileRecordsComponent>? ProfileRecords { get; set; }

        [JsonPropertyName("profileCollectibles")]
        public SingleComponentResponse<DestinyProfileCollectiblesComponent>? ProfileCollectibles { get; set; }

        [JsonPropertyName("profileTransitoryData")]
        public SingleComponentResponse<DestinyProfileTransitoryComponent>? ProfileTransitoryData { get; set; }

        [JsonPropertyName("metrics")]
        public SingleComponentResponse<DestinyMetricsComponent>? Metrics { get; set; }

        [JsonPropertyName("profileStringVariables")]
        public SingleComponentResponse<DestinyStringVariablesComponent>? ProfileStringVariables { get; set; }

        [JsonPropertyName("profileCommendations")]
        public SingleComponentResponse<DestinySocialCommendationsComponent>? ProfileCommendations { get; set; }

        // CHARACTER SCOPED COMPONENTS (Dictionary keyed by characterId)
        [JsonPropertyName("characters")]
        public DictionaryComponentResponseOfint64<DestinyCharacterComponent>? Characters { get; set; }

        [JsonPropertyName("characterInventories")]
        public DictionaryComponentResponseOfint64<DestinyInventoryComponent>? CharacterInventories { get; set; }

        [JsonPropertyName("characterLoadouts")]
        public DictionaryComponentResponseOfint64<DestinyLoadoutsComponent>? CharacterLoadouts { get; set; }

        [JsonPropertyName("characterProgressions")]
        public DictionaryComponentResponseOfint64<DestinyCharacterProgressionComponent>? CharacterProgressions { get; set; }

        [JsonPropertyName("characterRenderData")]
        public DictionaryComponentResponseOfint64<DestinyCharacterRenderComponent>? CharacterRenderData { get; set; }

        [JsonPropertyName("characterActivities")]
        public DictionaryComponentResponseOfint64<DestinyCharacterActivitiesComponent>? CharacterActivities { get; set; }

        [JsonPropertyName("characterEquipment")]
        public DictionaryComponentResponseOfint64<DestinyInventoryComponent>? CharacterEquipment { get; set; }

        [JsonPropertyName("characterKiosks")]
        public DictionaryComponentResponseOfint64<DestinyKiosksComponent>? CharacterKiosks { get; set; }

        [JsonPropertyName("characterPlugSets")]
        public DictionaryComponentResponseOfint64<DestinyPlugSetsComponent>? CharacterPlugSets { get; set; }

        // TODO: Implement DestinyBaseItemComponentSetOfuint32 if needed for characterUninstancedItemComponents
        // [JsonPropertyName("characterUninstancedItemComponents")]
        // public Dictionary<long, DestinyBaseItemComponentSetOfuint32> CharacterUninstancedItemComponents { get; set; }

        [JsonPropertyName("characterPresentationNodes")]
        public DictionaryComponentResponseOfint64<DestinyPresentationNodesComponent>? CharacterPresentationNodes { get; set; }

        [JsonPropertyName("characterRecords")]
        public DictionaryComponentResponseOfint64<DestinyCharacterRecordsComponent>? CharacterRecords { get; set; }

        [JsonPropertyName("characterCollectibles")]
        public DictionaryComponentResponseOfint64<DestinyCollectiblesComponent>? CharacterCollectibles { get; set; }

        [JsonPropertyName("characterStringVariables")]
        public DictionaryComponentResponseOfint64<DestinyStringVariablesComponent>? CharacterStringVariables { get; set; }

        [JsonPropertyName("characterCraftables")]
        public DictionaryComponentResponseOfint64<DestinyCraftablesComponent>? CharacterCraftables { get; set; }

        // ITEM INSTANCE COMPONENTS (Dictionary keyed by itemInstanceId)
        // This is a complex one: DestinyItemComponentSetOfint64
        // It contains multiple dictionaries, each for a specific item component type.
        [JsonPropertyName("itemComponents")]
        public DestinyItemComponentSetOfint64? ItemComponents { get; set; }

        // CHARACTER CURRENCY LOOKUPS
        [JsonPropertyName("characterCurrencyLookups")]
        public DictionaryComponentResponseOfint64<DestinyCurrenciesComponent>? CharacterCurrencyLookups { get; set; }
    }
}
