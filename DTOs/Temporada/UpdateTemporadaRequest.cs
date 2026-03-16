namespace persist_net_backend.DTOs.Temporada
{
    public class UpdateTemporadaRequest
    {
        public string? Nombre { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
    }
}
