// guardian-definitivo/src/Models/Destiny/Config/DestinyManifest.cs
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GuardianDefinitivo.Models.Destiny.Config
{
    /// <summary>
    /// Provides common properties for destiny definitions.
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Config-DestinyManifest.html#schema_Destiny-Config-DestinyManifest
    /// </summary>
    public class DestinyManifest
    {
        [JsonPropertyName("version")]
        public string? Version { get; set; }

        [JsonPropertyName("mobileAssetContentPath")]
        public string? MobileAssetContentPath { get; set; }

        [JsonPropertyName("mobileGearAssetDataBases")]
        public List<GearAssetDataBaseDefinition>? MobileGearAssetDataBases { get; set; }

        [JsonPropertyName("mobileWorldContentPaths")]
        public Dictionary<string, string>? MobileWorldContentPaths { get; set; }

        [JsonPropertyName("jsonWorldContentPaths")]
        public Dictionary<string, string>? JsonWorldContentPaths { get; set; }

        [JsonPropertyName("jsonWorldComponentContentPaths")]
        public Dictionary<string, Dictionary<string, string>>? JsonWorldComponentContentPaths { get; set; }

        [JsonPropertyName("mobileClanBannerDatabasePath")]
        public string? MobileClanBannerDatabasePath { get; set; }

        [JsonPropertyName("mobileGearCDN")]
        public Dictionary<string, string>? MobileGearCDN { get; set; }

        /// <summary>
        /// Information about the icons component content paths.
        /// </summary>
        [JsonPropertyName("iconImagePyramidInfo")]
        public List<ImagePyramidEntry>? IconImagePyramidInfo { get; set; }
    }

    /// <summary>
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Config-GearAssetDataBaseDefinition.html#schema_Destiny-Config-GearAssetDataBaseDefinition
    /// </summary>
    public class GearAssetDataBaseDefinition
    {
        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }
    }

    /// <summary>
    /// Sourced from: https://bungie-net.github.io/multi/schema_Destiny-Config-ImagePyramidEntry.html#schema_Destiny-Config-ImagePyramidEntry
    /// </summary>
    public class ImagePyramidEntry
    {
        /// <summary>
        /// The name of the subfolder where these images are located.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// The factor by which the original image size has been reduced.
        /// </summary>
        [JsonPropertyName("factor")]
        public float Factor { get; set; }
    }
}
