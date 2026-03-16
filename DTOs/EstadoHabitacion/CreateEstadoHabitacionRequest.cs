namespace persist_net_backend.DTOs.EstadoHabitacion
{
    public class CreateEstadoHabitacionRequest
    {
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; } = true;
    }
}
