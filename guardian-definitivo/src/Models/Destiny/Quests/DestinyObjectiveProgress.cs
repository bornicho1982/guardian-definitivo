// guardian-definitivo/src/Models/Destiny/Quests/DestinyObjectiveProgress.cs
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Quests
{
    /// <summary>
    /// Returns data about a character's status with a given Objective. Combine with DestinyObjectiveDefinition static data for display purposes.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Quests-DestinyObjectiveProgress.html#schema_Destiny-Quests-DestinyObjectiveProgress
    /// </summary>
    public class DestinyObjectiveProgress
    {
        /// <summary>
        /// The unique identifier of the Objective being tracked.
        /// </summary>
        [JsonPropertyName("objectiveHash")]
        public uint ObjectiveHash { get; set; }

        /// <summary>
        /// If the Objective has a Destination associated with it, this is the hash identifier of that Destination.
        /// Use it to look up the DestinyDestinationDefinition for static data. This will give you the Destination to show on maps, etc.
        /// </summary>
        [JsonPropertyName("destinationHash")]
        public uint? DestinationHash { get; set; }

        /// <summary>
        /// If the Objective has an Activity associated with it, this is the hash identifier of that Activity.
        /// Use it to look up the DestinyActivityDefinition for static data.
        /// </summary>
        [JsonPropertyName("activityHash")]
        public uint? ActivityHash { get; set; }

        /// <summary>
        /// If the objective has a progress description, this will be that description.
        /// </summary>
        [JsonPropertyName("progressDescription")]
        public string? ProgressDescription { get; set; } // This is not directly in the schema but often needed and available in definitions. For live data, it's usually part of the definition.

        /// <summary>
        /// If progress has been made, and the progress falls short of the Completion Value, this will be the current progress value.
        /// </summary>
        [JsonPropertyName("progress")]
        public int? Progress { get; set; }

        /// <summary>
        /// The value that the Progress variable needs to reach in order for the objective to be considered complete.
        /// </summary>
        [JsonPropertyName("completionValue")]
        public int CompletionValue { get; set; }

        /// <summary>
        /// Whether or not the Objective is completed.
        /// </summary>
        [JsonPropertyName("complete")]
        public bool Complete { get; set; }

        /// <summary>
        /// If this is true, the objective is visible in-game. Otherwise, it's not visible.
        /// </summary>
        [JsonPropertyName("visible")]
        public bool Visible { get; set; }
    }
}
