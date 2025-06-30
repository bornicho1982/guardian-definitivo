// guardian-definitivo/src/Models/GroupV2/GroupUserInfoCard.cs
using System.Collections.Generic;
using GuardianDefinitivo.Models.Enums; // Necesitará este using

namespace GuardianDefinitivo.Models.GroupV2 // Namespace ajustado
{
    public class GroupUserInfoCard
    {
        public string? LastSeenDisplayName { get; set; }
        public BungieMembershipType LastSeenDisplayNameType { get; set; }
        public string? iconPath { get; set; }
        public BungieMembershipType crossSaveOverride { get; set; } // Originalmente int, pero es un BungieMembershipType
        public List<BungieMembershipType>? applicableMembershipTypes { get; set; } // Originalmente List<int>
        public bool isPublic { get; set; }
        public BungieMembershipType membershipType { get; set; }
        public long membershipId { get; set; } // string en formato long
        public string? displayName { get; set; }
        public string? bungieGlobalDisplayName { get; set; }
        public short? bungieGlobalDisplayNameCode { get; set; }
    }
}
