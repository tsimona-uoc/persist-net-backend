using persist_net_backend.DTOs.EstadoHabitacion;
using persist_net_backend.DTOs.TipoHabitacion;

namespace persist_net_backend.DTOs.Habitacion
{
    public class HabitacionResponse
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int Planta { get; set; }
        public int Numero { get; set; }
        public required TipoHabitacionResponse TipoHabitacion { get; set; }
        public required EstadoHabitacionResponse EstadoHabitacion { get; set; }
    }
}
