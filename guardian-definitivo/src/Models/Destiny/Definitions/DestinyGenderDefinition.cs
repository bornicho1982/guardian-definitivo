// guardian-definitivo/src/Models/Destiny/Definitions/DestinyGenderDefinition.cs
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Definitions.Common;

namespace GuardianDefinitivo.Models.Destiny.Definitions
{
    /// <summary>
    /// Gender is a social construct, and as such we have definitions for Genders. Right now there are only two,
    /// but we may expand this sometime in the future.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Definitions-DestinyGenderDefinition.html#schema_Destiny-Definitions-DestinyGenderDefinition
    /// </summary>
    public class DestinyGenderDefinition : DestinyDefinition
    {
        [JsonPropertyName("displayProperties")]
        public DestinyDisplayPropertiesDefinition? DisplayProperties { get; set; }

        /// <summary>
        /// This is a quick reference enumeration for all of the currently defined Genders.
        /// We use this solely to map the Gender values stored in the database to a conceptual values.
        /// </summary>
        [JsonPropertyName("genderType")]
        public int GenderType { get; set; } // Enum: DestinyGender
    }
}
