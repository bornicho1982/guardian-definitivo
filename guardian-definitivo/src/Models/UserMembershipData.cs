// guardian-definitivo/src/Models/UserMembershipData.cs
using System.Collections.Generic;
using GuardianDefinitivo.Models.GroupV2; // Necesitará este using
using GuardianDefinitivo.Models.User; // Necesitará este using

namespace GuardianDefinitivo.Models // Cambiado de Data a Models
{
    public class UserMembershipData
    {
        public List<GroupUserInfoCard>? destinyMemberships { get; set; }
        public long? primaryMembershipId { get; set; } // string en formato long
        public BungieNetUserInfo? bungieNetUser { get; set; }
    }
}

// Mover las otras clases a sus respectivos archivos también.
// Este archivo solo contendrá UserMembershipData.
