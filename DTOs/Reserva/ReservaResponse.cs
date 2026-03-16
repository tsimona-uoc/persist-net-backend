using persist_net_backend.DTOs.Regimen;
using persist_net_backend.DTOs.EstadoReserva;

namespace persist_net_backend.DTOs.Reserva
{
    public class ReservaResponse
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int HabitacionId { get; set; }
        public DateOnly FechaEntrada { get; set; }
        public DateOnly FechaSalida { get; set; }
        public required RegimenResponse Regimen { get; set; }
        public required EstadoReservaResponse EstadoReserva { get; set; }
        public decimal PrecioActual { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
