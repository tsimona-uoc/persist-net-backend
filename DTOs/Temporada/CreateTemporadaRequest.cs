namespace persist_net_backend.DTOs.Temporada
{
    public class CreateTemporadaRequest
    {
        public required string Nombre { get; set; }
        public required DateOnly FechaInicio { get; set; }
        public required DateOnly FechaFin { get; set; }
    }
}
