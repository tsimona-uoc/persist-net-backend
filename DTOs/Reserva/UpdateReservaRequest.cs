namespace persist_net_backend.DTOs.Reserva
{
    public class UpdateReservaRequest
    {
        public int? ClienteId { get; set; }
        public int? HabitacionId { get; set; }
        public DateOnly? FechaEntrada { get; set; }
        public DateOnly? FechaSalida { get; set; }
        public int? RegimenId { get; set; }
        public int? EstadoReservaId { get; set; }
    }
}
