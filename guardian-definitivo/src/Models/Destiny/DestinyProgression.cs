// guardian-definitivo/src/Models/Destiny/DestinyProgression.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny
{
    /// <summary>
    /// Information about a current character's status with a Progression. A progression is a value that can increase with activity and has levels.
    /// Think Character Level, or Reputation Levels. progressions are defined in a DestinyProgressionDefinition.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-DestinyProgression.html#schema_Destiny-DestinyProgression
    /// </summary>
    public class DestinyProgression
    {
        /// <summary>
        /// The hash identifier of the Progression in question. Use it to look up the DestinyProgressionDefinition in static data.
        /// </summary>
        [JsonPropertyName("progressionHash")]
        public uint ProgressionHash { get; set; }

        /// <summary>
        /// The amount of progress earned today.
        /// </summary>
        [JsonPropertyName("dailyProgress")]
        public int DailyProgress { get; set; }

        /// <summary>
        /// If this progression has a daily limit, this is that limit.
        /// </summary>
        [JsonPropertyName("dailyLimit")]
        public int DailyLimit { get; set; }

        /// <summary>
        /// The amount of progress earned toward this progression in the current week.
        /// </summary>
        [JsonPropertyName("weeklyProgress")]
        public int WeeklyProgress { get; set; }

        /// <summary>
        /// If this progression has a weekly limit, this is that limit.
        /// </summary>
        [JsonPropertyName("weeklyLimit")]
        public int WeeklyLimit { get; set; }

        /// <summary>
        /// This is the total amount of progress obtained overall for this progression (for instance, the total amount of Character Level experience earned)
        /// </summary>
        [JsonPropertyName("currentProgress")]
        public int CurrentProgress { get; set; }

        /// <summary>
        /// This is the level of the progression (for instance, the Character Level).
        /// </summary>
        [JsonPropertyName("level")]
        public int Level { get; set; }

        /// <summary>
        /// This is the maximum possible level you can achieve for this progression (for example, the maximum character level obtainable)
        /// </summary>
        [JsonPropertyName("levelCap")]
        public int LevelCap { get; set; }

        /// <summary>
        /// Progressions define their levels in "steps". Since the last step may be repeatable, the user may be at a higher level than the actual maximum level definition indicates.
        /// With Destiny 2 current UI, we don't have a concept of levels over Max Level, so this is providing unused data.
        /// </summary>
        [JsonPropertyName("stepIndex")]
        public int StepIndex { get; set; }

        /// <summary>
        /// The amount of progression (i.e. "Experience") needed to reach the next level of this Progression. Jeez, progression is such an overloaded word.
        /// </summary>
        [JsonPropertyName("progressToNextLevel")]
        public int ProgressToNextLevel { get; set; }

        /// <summary>
        /// The total amount of progression (i.e. "Experience") needed to reach the next level of this Progression from the beginning of the current level.
        /// </summary>
        [JsonPropertyName("nextLevelAt")]
        public int NextLevelAt { get; set; }

        /// <summary>
        /// The number of resets of this progression you've executed this season, if applicable to this progression.
        /// </summary>
        [JsonPropertyName("currentResetCount")]
        public int? CurrentResetCount { get; set; }

        /// <summary>
        /// Information about historical resets of this progression, if applicable to this progression.
        /// </summary>
        [JsonPropertyName("seasonResets")]
        public List<DestinyProgressionResetEntry>? SeasonResets { get; set; } // DestinyProgressionResetEntry model needed

        /// <summary>
        /// Information about historical rewards for this progression, if applicable to this progression.
        /// </summary>
        [JsonPropertyName("rewardItemStates")]
        public List<int>? RewardItemStates { get; set; } // List of DestinyProgressionRewardItemState (enum)
    }

    /// <summary>
    /// Represents a season and the number of resets you had in that season.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-DestinyProgressionResetEntry.html#schema_Destiny-DestinyProgressionResetEntry
    /// </summary>
    public class DestinyProgressionResetEntry
    {
        [JsonPropertyName("season")]
        public int Season { get; set; }

        [JsonPropertyName("resets")]
        public int Resets { get; set; }
    }
}
