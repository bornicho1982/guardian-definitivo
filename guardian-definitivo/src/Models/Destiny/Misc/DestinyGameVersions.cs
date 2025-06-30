// guardian-definitivo/src/Models/Destiny/Misc/DestinyGameVersions.cs
using System;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Misc
{
    /// <summary>
    /// A flags enumeration indicating the versions of the game that a given user has purchased.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-DestinyGameVersions.html#schema_Destiny-DestinyGameVersions
    /// </summary>
    [Flags]
    [JsonConverter(typeof(JsonStringEnumConverter))] // Assuming it might be represented as a string or number in JSON, this helps if it's a string.
                                                     // However, the API returns an int. A custom converter might be needed if direct int mapping to Flags enum is problematic with System.Text.Json.
                                                     // For now, we'll assume it's handled as an int.
    public enum DestinyGameVersions
    {
        None = 0,
        Destiny2 = 1,
        DLC1 = 2, // Curse of Osiris
        DLC2 = 4, // Warmind
        Forsaken = 8,
        YearTwoAnnualPass = 16,
        Shadowkeep = 32,
        BeyondLight = 64,
        TheWitchQueen = 128,
        Lightfall = 256,
        TheFinalShape = 512 // Added for future proofing, check actual value when API updates
        // Future DLCs will be powers of 2
    }
}
