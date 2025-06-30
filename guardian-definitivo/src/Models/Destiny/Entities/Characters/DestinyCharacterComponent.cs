// guardian-definitivo/src/Models/Destiny/Entities/Characters/DestinyCharacterComponent.cs
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GuardianDefinitivo.Models.Destiny.Misc; // For DestinyColor, DestinyGameVersions

namespace GuardianDefinitivo.Models.Destiny.Entities.Characters
{
    /// <summary>
    /// This component contains base properties of the character. You'll probably want to always request this component,
    /// but hey you do you.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Entities-Characters-DestinyCharacterComponent.html#schema_Destiny-Entities-Characters-DestinyCharacterComponent
    /// </summary>
    public class DestinyCharacterComponent
    {
        /// <summary>
        /// Every Destiny Profile has a membershipId. This is provided on the Profile level.
        /// This is the silver variable, though. Destiny Profile can have multiple memberships (and they can be different accounts).
        /// </summary>
        [JsonPropertyName("membershipId")]
        public long MembershipId { get; set; }

        /// <summary>
        /// membershipType tells you the platform on which this character plays. Examine the BungieMembershipType enumeration for possible values.
        /// </summary>
        [JsonPropertyName("membershipType")]
        public int MembershipType { get; set; } // BungieMembershipType enum

        /// <summary>
        /// The unique identifier for the character.
        /// </summary>
        [JsonPropertyName("characterId")]
        public long CharacterId { get; set; }

        /// <summary>
        /// The last date that the user played Destiny.
        /// </summary>
        [JsonPropertyName("dateLastPlayed")]
        public DateTime DateLastPlayed { get; set; }

        /// <summary>
        /// If the user is currently playing, this is how long they've been playing.
        /// </summary>
        [JsonPropertyName("minutesPlayedThisSession")]
        public long MinutesPlayedThisSession { get; set; } // Represented as string in JSON, convert to long

        /// <summary>
        /// If this character has been played ever in the game, this is the total number of minutes played.
        /// </summary>
        [JsonPropertyName("minutesPlayedTotal")]
        public long MinutesPlayedTotal { get; set; } // Represented as string in JSON, convert to long

        /// <summary>
        /// The user's calculated "Light Level". Light level is an indicator of your power that mostly matters in the end game, once you've reached the maximum character level: it's a level that's dependent on the average Attack/Defense power of your items.
        /// </summary>
        [JsonPropertyName("light")]
        public int Light { get; set; }

        /// <summary>
        /// Your character's stats, such as Agility, Resilience, etc... *not* historical stats.
        /// You'll have to call a different endpoint for those.
        /// </summary>
        [JsonPropertyName("stats")]
        public Dictionary<uint, int>? Stats { get; set; } // Keyed by Stat Hash

        /// <summary>
        /// Use this hash to look up the character's DestinyRaceDefinition.
        /// </summary>
        [JsonPropertyName("raceHash")]
        public uint RaceHash { get; set; }

        /// <summary>
        /// Use this hash to look up the character's DestinyGenderDefinition.
        /// </summary>
        [JsonPropertyName("genderHash")]
        public uint GenderHash { get; set; }

        /// <summary>
        /// Use this hash to look up the character's DestinyClassDefinition.
        /// </summary>
        [JsonPropertyName("classHash")]
        public uint ClassHash { get; set; }

        /// <summary>
        /// The hash of the currently equipped emblem for the user. Can be used to look up the DestinyInventoryItemDefinition.
        /// </summary>
        [JsonPropertyName("emblemHash")]
        public uint EmblemHash { get; set; }

        /// <summary>
        /// The hash of the currently equipped emblem background for the user. Can be used to look up the DestinyInventoryItemDefinition.
        /// </summary>
        [JsonPropertyName("emblemBackgroundPath")]
        public string? EmblemBackgroundPath { get; set; }

        /// <summary>
        /// The path of the emblem used by this character. This will be the icon in the manifest's DestinyInventoryItemDefinition for the item identified by the emblemHash property.
        /// </summary>
        [JsonPropertyName("emblemPath")]
        public string? EmblemPath { get; set; }

        /// <summary>
        /// The color of the emblem used by this character. (this is a DestinyColor)
        /// </summary>
        [JsonPropertyName("emblemColor")]
        public DestinyColor? EmblemColor { get; set; }

        /// <summary>
        /// The progression that indicates your character's level. Not their light level, but their character level: you know, the thing you max out a couple hours in and then ignore for the sake of light level.
        /// </summary>
        [JsonPropertyName("levelProgression")]
        public DestinyProgression? LevelProgression { get; set; } // Needs Destiny.DestinyProgression model

        /// <summary>
        /// The "base" level of your character, not accounting for any light level.
        /// </summary>
        [JsonPropertyName("baseCharacterLevel")]
        public int BaseCharacterLevel { get; set; }

        /// <summary>
        /// A number between 0 and 100, indicating the whole and fractional % remaining to get to the next character level.
        /// </summary>
        [JsonPropertyName("percentToNextLevel")]
        public float PercentToNextLevel { get; set; }

        /// <summary>
        /// The bungie global display name for the character.
        /// </summary>
        [JsonPropertyName("bungieGlobalDisplayName")]
        public string? BungieGlobalDisplayName { get; set; }

        /// <summary>
        /// The bungie global display name code for the character.
        /// </summary>
        [JsonPropertyName("bungieGlobalDisplayNameCode")]
        public short? BungieGlobalDisplayNameCode { get; set; }

        /// <summary>
        /// The title item equipped on the character, if any.
        /// </summary>
        [JsonPropertyName("titleRecordHash")]
        public uint? TitleRecordHash { get; set; }
    }
}
