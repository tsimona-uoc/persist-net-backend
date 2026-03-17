namespace persist_net_backend.DTOs.Estancia
{
    public class UpdateEstanciaRequest
    {
        public int? ReservaId { get; set; }
        public DateTime? FechaCheckIn { get; set; }
        public DateTime? FechaCheckOut { get; set; }
        public int? EstadoEstanciaId { get; set; }
    }
}