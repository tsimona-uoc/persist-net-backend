namespace persist_net_backend.DTOs.EstadoHabitacion
{
    public class UpdateEstadoHabitacionRequest
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool? Activo { get; set; }
    }
}
