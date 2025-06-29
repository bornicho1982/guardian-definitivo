namespace GuardianDefinitivo.Modules.Clans
{
    public class ClanFinder
    {
        public List<Clan> BuscarClanes(string idioma, string estilo)
        {
            return new List<Clan>
            {
                new Clan { Nombre = "NovaGuard", Miembros = 78, RequiereMicro = true, Enlace = "https://www.bungie.net/Clan/Detail/123456" },
                new Clan { Nombre = "Forjadores del Vacío", Miembros = 102, RequiereMicro = false, Enlace = "https://www.bungie.net/Clan/Detail/654321" }
            };
        }
    }

    public class Clan
    {
        public string Nombre { get; set; }
        public int Miembros { get; set; }
        public bool RequiereMicro { get; set; }
        public string Enlace { get; set; }
    }
}