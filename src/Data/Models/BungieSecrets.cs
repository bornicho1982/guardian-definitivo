namespace GuardianDefinitivo.Data.Models
{
    public class BungieSecrets
    {
        public BungieInfo? Bungie { get; set; }
    }

    public class BungieInfo
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? ApiKey { get; set; }
        public string? RedirectUri { get; set; }
    }
}
