namespace GuardianDefinitivo.Modules.Builds
{
    public class BuildOptimizer
    {
        public BuildResult OptimizarTriple100(BuildInput input)
        {
            // Aquí va la lógica para calcular si con el inventario actual puedes llegar al triple-100
            return new BuildResult
            {
                Alcanzable = false,
                Faltantes = new List<string> { "Casco +26 Disciplina", "Botas +20 Recuperación" },
                RecomendacionesFarmeo = new List<string> { "Sector perdido (Jueves): Casco legendario", "Evento: Acometida" }
            };
        }
    }

    public class BuildInput
    {
        public string Clase { get; set; }
        public string Subclase { get; set; }
        public List<Item> Inventario { get; set; }
    }

    public class BuildResult
    {
        public bool Alcanzable { get; set; }
        public List<string> Faltantes { get; set; }
        public List<string> RecomendacionesFarmeo { get; set; }
    }
}