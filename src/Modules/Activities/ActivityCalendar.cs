namespace GuardianDefinitivo.Modules.Activities
{
    public class ActivityCalendar
    {
        public List<Activity> ObtenerActividadesDeHoy()
        {
            return new List<Activity>
            {
                new Activity { Nombre = "Ocaso GM: La Vanguardia", Tipo = "PvE", Loot = "Exótico aleatorio", Recomendacion = "Resistencia + Recuperación alta" },
                new Activity { Nombre = "Raid: Jardín de la Salvación", Tipo = "Incursión", Loot = "Botas de Sabiduría del Jardín" }
            };
        }
    }

    public class Activity
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Loot { get; set; }
        public string Recomendacion { get; set; }
    }
}