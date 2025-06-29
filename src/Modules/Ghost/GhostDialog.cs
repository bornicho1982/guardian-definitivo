namespace GuardianDefinitivo.Modules.Ghost
{
    public class GhostDialog
    {
        public string Responder(string entrada)
        {
            if (entrada.Contains("build pvp"))
                return "Sugiero un exótico como 'Wormhusk' con movilidad alta y doble mod de recuperación.";

            if (entrada.Contains("farmear disciplina"))
                return "Busca armaduras con afinidad solar en sectores perdidos hoy: tendrás más posibilidad de que caigan piezas útiles.";

            return "¿Podrías reformular tu pregunta, Guardián?";
        }
    }
}