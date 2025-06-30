// guardian-definitivo/src/Models/User/BungieNetUserInfo.cs

namespace GuardianDefinitivo.Models.User // Namespace ajustado
{
    public class BungieNetUserInfo
    {
        public long membershipId { get; set; } // string en formato long
        public string? uniqueName { get; set; }
        public string? displayName { get; set; }
        public int profilePicture { get; set; } // ID de imagen de perfil de Bungie.net
        public int profileTheme { get; set; } // ID de tema de perfil de Bungie.net
        public int userTitle { get; set; } // ID de título de usuario de Bungie.net
        public long? successMessageFlags { get; set; } // string en formato long
        public bool? isDeleted { get; set; }
        public string? about { get; set; }
        public DateTime? firstAccess { get; set; }
        public DateTime? lastUpdate { get; set; }
        public long? legacyPortalUID { get; set; } // string en formato long
        public string? context { get; set; } // Podría ser UserContext
        public string? psnDisplayName { get; set; }
        public string? xboxDisplayName { get; set; }
        public string? fbDisplayName { get; set; }
        public bool? showActivity { get; set; }
        public string? locale { get; set; }
        public bool? localeInheritDefault { get; set; }
        public long? lastBanReportId { get; set; } // string en formato long
        public bool? showGroupMessaging { get; set; }
        public string? blizzardDisplayName { get; set; }
        public string? steamDisplayName { get; set; }
        public string? stadiaDisplayName { get; set; }
        public string? twitchDisplayName { get; set; }
        public string? cachedBungieGlobalDisplayName { get; set; }
        public short? cachedBungieGlobalDisplayNameCode { get; set; }
        public string? egsDisplayName { get; set; }
    }
}
