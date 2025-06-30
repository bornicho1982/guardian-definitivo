// guardian-definitivo/src/Models/Enums/BungieMembershipType.cs

namespace GuardianDefinitivo.Models.Enums
{
    public enum BungieMembershipType
    {
        None = 0,
        TigerXbox = 1,
        TigerPsn = 2,
        TigerSteam = 3,
        TigerBlizzard = 4,  // Obsoleto
        TigerStadia = 5,    // Obsoleto
        TigerEgs = 6,       // Epic Games Store
        TigerDemon = 10,    // Usado internamente por Bungie para pruebas
        BungieNext = 254,   // Bungie.net
        All = -1            // Representa todas las plataformas en búsquedas, etc.
    }
}
