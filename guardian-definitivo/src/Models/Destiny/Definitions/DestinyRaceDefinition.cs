// guardian-definitivo/src/Models/Destiny/Definitions/DestinyRaceDefinition.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Definitions.Common;

namespace GuardianDefinitivo.Models.Destiny.Definitions
{
    /// <summary>
    /// In Destiny, "Races" are really more like "Species". Sort of. I mean, are the Awoken a separate species from humans? I'm not sure.
    /// But either way, they're defined here. You'll see Exo, Awoken, and Human as examples of these.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyRaceDefinition.html#schema_Destiny-Definitions-DestinyRaceDefinition
    /// </summary>
    public class DestinyRaceDefinition : DestinyDefinition
    {
        [JsonPropertyName("displayProperties")]
        public DestinyDisplayPropertiesDefinition? DisplayProperties { get; set; }

        /// <summary>
        /// An enumeration defining the existing, known Races in Destiny.
        /// </summary>
        [JsonPropertyName("raceType")]
        public int RaceType { get; set; } // Enum: DestinyRace

        /// <summary>
        /// A localized string referring to the singular form of the Race's name when referred to in gendered form.
        /// Keyed by the DestinyGenderDefinition hash.
        /// </summary>
        [JsonPropertyName("genderedRaceNames")]
        public Dictionary<uint, string>? GenderedRaceNames { get; set; } // Key is GenderHash

        [JsonPropertyName("genderedRaceNamesByGenderHash")] // Redundant with above but sometimes present
        public Dictionary<string, string>? GenderedRaceNamesByGenderHash { get; set; } // Key is GenderHash as string
    }
}
