// guardian-definitivo/src/Models/Destiny/Definitions/Sockets/DestinySocketTypeDefinition.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Definitions.Common;

namespace GuardianDefinitivo.Models.Destiny.Definitions.Sockets
{
    /// <summary>
    /// All Sockets have a type, and this is the definition of those types.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-Sockets-DestinySocketTypeDefinition.html#schema_Destiny-Definitions-Sockets-DestinySocketTypeDefinition
    /// </summary>
    public class DestinySocketTypeDefinition : DestinyDefinition
    {
        [JsonPropertyName("displayProperties")]
        public DestinyDisplayPropertiesDefinition? DisplayProperties { get; set; }

        /// <summary>
        /// Defines how plugs visually appear in sockets of this type.
        /// </summary>
        [JsonPropertyName("insertAction")]
        public DestinyInsertPlugActionDefinition? InsertAction { get; set; } // DestinyInsertPlugActionDefinition needed

        /// <summary>
        /// A list of Plug Whitelist Entries that represent the valid plugs that can be inserted into this socket, ignoring other rules such as levels and equipping/visibility constraints.
        /// </summary>
        [JsonPropertyName("plugWhitelist")]
        public List<DestinyPlugWhitelistEntryDefinition>? PlugWhitelist { get; set; } // DestinyPlugWhitelistEntryDefinition needed

        [JsonPropertyName("socketCategoryHash")]
        public uint SocketCategoryHash { get; set; }

        /// <summary>
        /// Sometimes a socket isn't visible. These are some of the conditions under which it may be hidden.
        /// </summary>
        [JsonPropertyName("visibility")]
        public int Visibility { get; set; } // Enum: DestinySocketVisibility (0=Visible, 1=Hidden, 2=HiddenWhenEmpty, 3=HiddenIfNoPlugsAvailable)

        [JsonPropertyName("alwaysRandomizeSockets")]
        public bool AlwaysRandomizeSockets { get; set; }

        [JsonPropertyName("isPreviewEnabled")]
        public bool IsPreviewEnabled { get; set; }

        [JsonPropertyName("hideDuplicateReusablePlugs")]
        public bool HideDuplicateReusablePlugs { get; set; }

        /// <summary>
        /// This property indicates if the socket type determines the power level of the weapon.
        /// </summary>
        [JsonPropertyName("overridesUiAppearance")]
        public bool OverridesUiAppearance { get; set; }

        [JsonPropertyName("avoidDuplicatesOnInitialization")]
        public bool AvoidDuplicatesOnInitialization { get; set; }

        [JsonPropertyName("currencyScalars")]
        public List<DestinySocketTypeScalarMaterialRequirementEntry>? CurrencyScalars { get; set; } // DestinySocketTypeScalarMaterialRequirementEntry needed
    }

    // Minimal placeholder definitions for nested types, can be expanded if needed.
    public class DestinyInsertPlugActionDefinition
    {
        [JsonPropertyName("actionExecuteSeconds")]
        public int ActionExecuteSeconds { get; set; }
        [JsonPropertyName("actionType")]
        public int ActionType {get; set;} // Enum: SocketTypeActionType
    }

    public class DestinyPlugWhitelistEntryDefinition
    {
        [JsonPropertyName("categoryHash")]
        public uint CategoryHash { get; set; }
        [JsonPropertyName("categoryIdentifier")]
        public string? CategoryIdentifier { get; set; }
        // [JsonPropertyName("reinitializationPossiblePlugHashes")]
        // public List<uint> ReinitializationPossiblePlugHashes { get; set; } // Not always present
    }

    public class DestinySocketTypeScalarMaterialRequirementEntry
    {
        [JsonPropertyName("currencyItemHash")]
        public uint CurrencyItemHash { get; set; }
        [JsonPropertyName("scalarValue")]
        public int ScalarValue { get; set; }
    }
}
