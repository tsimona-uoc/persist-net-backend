namespace persist_net_backend.DTOs.Habitacion
{
    public class UpdateHabitacionRequest
    {
        public int? HotelId { get; set; }
        public int? TipoHabitacionId { get; set; }
        public int? Planta { get; set; }
        public int? Numero { get; set; }
        public int? EstadoHabitacionId { get; set; }
    }
}
