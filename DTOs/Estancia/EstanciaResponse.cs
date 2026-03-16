using persist_net_backend.DTOs.EstadoEstancia;

namespace persist_net_backend.DTOs.Estancia
{
    public class EstanciaResponse
    {
        public int Id { get; set; }
        public int ReservaId { get; set; }
        public DateTime FechaCheckIn { get; set; }
        public DateTime? FechaCheckOut { get; set; }
        public required EstadoEstanciaResponse EstadoEstancia { get; set; }
    }
}
