namespace persist_net_backend.DTOs.Temporada
{
    public class TemporadaResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
    }
}
