namespace persist_net_backend.DTOs.Reserva
{
    public class CreateReservaRequest
    {
        public required int ClienteId { get; set; }
        public required int HabitacionId { get; set; }
        public required DateOnly FechaEntrada { get; set; }
        public required DateOnly FechaSalida { get; set; }
        public required int RegimenId { get; set; }
        public required int EstadoReservaId { get; set; }
    }
}
