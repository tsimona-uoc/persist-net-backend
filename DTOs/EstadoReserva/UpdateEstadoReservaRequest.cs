namespace persist_net_backend.DTOs.EstadoReserva
{
    public class UpdateEstadoReservaRequest
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool? Activo { get; set; }
    }
}
