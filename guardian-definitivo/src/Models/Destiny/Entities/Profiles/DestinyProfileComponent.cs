// guardian-definitivo/src/Models/Destiny/Entities/Profiles/DestinyProfileComponent.cs
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.User; // For UserInfoCard
using GuardianDefinitivo.Models.Destiny.Misc; // For DestinyGameVersions

namespace GuardianDefinitivo.Models.Destiny.Entities.Profiles
{
    /// <summary>
    /// The most essential summary information about a Profile (Destiny account).
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Entities-Profiles-DestinyProfileComponent.html#schema_Destiny-Entities-Profiles-DestinyProfileComponent
    /// </summary>
    public class DestinyProfileComponent
    {
        /// <summary>
        /// If you need to render the Profile (their platform name, icon, etc...) somewhere, this is the data for doing so.
        /// </summary>
        [JsonPropertyName("userInfo")]
        public UserInfoCard? UserInfo { get; set; } // From Models.User

        /// <summary>
        /// The last time the user played with any character on this Profile.
        /// </summary>
        [JsonPropertyName("dateLastPlayed")]
        public DateTime DateLastPlayed { get; set; }

        /// <summary>
        /// If this profile is being overridden/obscured by Cross Save, this will be set to true.
        /// We will still return the profile for display purposes (showing the overridden name, etc)
        /// but we will not return any sensitive data (owned items, progression, etc).
        /// </summary>
        [JsonPropertyName("isCrossSavePrimary")]
        public bool IsCrossSavePrimary { get; set; }

        /// <summary>
        /// If this profile is a cross save override on either this or another platform,
        /// this field will be populated with the platform membership type that designates the primary account.
        /// </summary>
        [JsonPropertyName("crossSaveOverride")]
        public int? CrossSaveOverride { get; set; } // Represents BungieMembershipType

        /// <summary>
        /// The list of membership types indicating the platforms on which this Profile plays destiny.
        /// </summary>
        [JsonPropertyName("applicableMembershipTypes")]
        public List<int>? ApplicableMembershipTypes { get; set; } // List of BungieMembershipType

        /// <summary>
        /// If true, this profile is played on a platform where Silver is enabled for sale.
        /// </summary>
        [JsonPropertyName("isSilverPrimary")]
        public bool IsSilverPrimary { get; set; }

        /// <summary>
        /// A list of the seasons that this profile owns.
        /// Unlike the live DestinyAccountComponent implementation, this value actually returns the season passes that are owned.
        /// </summary>
        [JsonPropertyName("seasonHashes")]
        public List<uint>? SeasonHashes { get; set; }

        /// <summary>
        /// A list of hashes for event cards that a profile owns. Unlike the live DestinyAccountComponent implementation, this value actually returns the event cards that are owned.
        /// </summary>
        [JsonPropertyName("eventCardHashesOwned")]
        public List<uint>? EventCardHashesOwned { get; set; }

        /// <summary>
        /// A list of hashes for Guardian Ranks that a profile has earned.
        /// </summary>
        [JsonPropertyName("guardianRankHashes")]
        public List<uint>? GuardianRankHashes { get; set; }

        /// <summary>
        /// The version of the game that this profile last played.
        /// </summary>
        [JsonPropertyName("versionsOwned")]
        public DestinyGameVersions VersionsOwned { get; set; } // Enum

        /// <summary>
        /// If populated, this is a reference to the season that is currently active.
        /// </summary>
        [JsonPropertyName("currentSeasonHash")]
        public uint? CurrentSeasonHash { get; set; }

        /// <summary>
        /// If populated, this is a reference to the event card that is currently active.
        /// </summary>
        [JsonPropertyName("currentEventCardHash")]
        public uint? CurrentEventCardHash { get; set; }

        /// <summary>
        /// The highest guardian rank achieved by this account.
        /// </summary>
        [JsonPropertyName("currentGuardianRank")]
        public int CurrentGuardianRank { get; set; }

        /// <summary>
        /// The lifetime highest guardian rank achieved by this account.
        /// </summary>
        [JsonPropertyName("lifetimeHighestGuardianRank")]
        public int LifetimeHighestGuardianRank { get; set; }

        /// <summary>
        /// This profile's chosen name.
        /// </summary>
        [JsonPropertyName("bungieGlobalDisplayName")]
        public string? BungieGlobalDisplayName { get; set; }

        [JsonPropertyName("bungieGlobalDisplayNameCode")]
        public short? BungieGlobalDisplayNameCode { get; set; }

        /// <summary>
        /// A list of character IDs for the Destiny 2 characters on this account.
        /// </summary>
        [JsonPropertyName("characterIds")]
        public List<long>? CharacterIds { get; set; } // List of Int64
    }
}
