namespace persist_net_backend.DTOs.Estancia
{
    public class CreateEstanciaRequest
    {
        public required int ReservaId { get; set; }
        public required DateTime FechaCheckIn { get; set; }
        public DateTime? FechaCheckOut { get; set; }
        public int EstadoEstanciaId { get; set; } = 1; // Default: ACTIVA
    }
}