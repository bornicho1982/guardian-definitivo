// guardian-definitivo/src/Models/Destiny/Definitions/DestinyStatGroupDefinition.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Definitions
{
    /// <summary>
    /// When an inventory item (DestinyInventoryItemDefinition) has Stats (such as Attack Power),
    /// the item will refer to a DestinyStatGroupDefinition defined herein that contains the native stats
    /// (as DestinyStatDefinition definitions) that it will display to the user.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyStatGroupDefinition.html#schema_Destiny-Definitions-DestinyStatGroupDefinition
    /// </summary>
    public class DestinyStatGroupDefinition : DestinyDefinition
    {
        /// <summary>
        /// The maximum possible value that any stat in this group can be rendered.
        /// </summary>
        [JsonPropertyName("maximumValue")]
        public int MaximumValue { get; set; }

        /// <summary>
        /// This apparently indicates the type of UI visual style that will be used for this stat group.
        /// </summary>
        [JsonPropertyName("uiPosition")]
        public int UiPosition { get; set; }

        /// <summary>
        /// Any stat that appears in this group is allowed to be scaled.
        /// </summary>
        [JsonPropertyName("scaledStats")]
        public List<DestinyStatDisplayDefinition>? ScaledStats { get; set; } // DestinyStatDisplayDefinition needed

        /// <summary>
        /// The game has the ability to override, based on the stat group, what the localized text is that is displayed for Stats being shown on the item.
        /// Mercifully, no Stat Groups use this feature currently. If they start using them, we'll have to deal with the implications.
        /// </summary>
        [JsonPropertyName("overrides")]
        public Dictionary<uint, DestinyStatOverrideDefinition>? Overrides { get; set; } // Key is statHash, DestinyStatOverrideDefinition needed
    }

    /// <summary>
    /// Describes the way that an Item Stat (see DestinyStatDefinition) is transformed using the DestinyStatGroupDefinition related to that item.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyStatDisplayDefinition.html#schema_Destiny-Definitions-DestinyStatDisplayDefinition
    /// </summary>
    public class DestinyStatDisplayDefinition
    {
        /// <summary>
        /// The hash identifier for the stat being transformed.
        /// </summary>
        [JsonPropertyName("statHash")]
        public uint StatHash { get; set; }

        /// <summary>
        /// Regardless of the output of interpolation, this is the maximum possible value that the stat can be.
        /// It should also be equal to the upper bound of your visible bar charts/gauges if you use any.
        /// </summary>
        [JsonPropertyName("maximumValue")]
        public int MaximumValue { get; set; }

        /// <summary>
        /// If this is true, the stat should be displayed as a number. Otherwise, display it as a progress bar.
        /// </summary>
        [JsonPropertyName("displayAsNumeric")]
        public bool DisplayAsNumeric { get; set; }

        /// <summary>
        /// The interpolation table representing how the Investment Stat is transformed into a Display Stat.
        /// </summary>
        [JsonPropertyName("displayInterpolation")]
        public List<Interpolation.InterpolationPoint>? DisplayInterpolation { get; set; } // InterpolationPoint needed
    }

    // Placeholder for DestinyStatOverrideDefinition
    public class DestinyStatOverrideDefinition
    {
        [JsonPropertyName("statHash")]
        public uint StatHash { get; set; }
        // More properties if needed, like displayProperties
    }
}

// Namespace for InterpolationPoint, as it's used by DestinyStatDisplayDefinition
namespace GuardianDefinitivo.Models.Destiny.Definitions.Interpolation
{
    public class InterpolationPoint
    {
        [JsonPropertyName("value")]
        public int Value { get; set; }
        [JsonPropertyName("weight")]
        public int Weight { get; set; }
    }
}
