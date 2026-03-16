namespace persist_net_backend.DTOs.Habitacion
{
    public class CreateHabitacionRequest
    {
        public required int HotelId { get; set; }
        public required int TipoHabitacionId { get; set; }
        public required int Planta { get; set; }
        public required int Numero { get; set; }
        public int EstadoHabitacionId { get; set; } = 1; // Default: LIBRE
    }
}
