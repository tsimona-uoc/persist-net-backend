namespace persist_net_backend.DTOs.TipoHabitacion
{
    public class CreateTipoHabitacionRequest
    {
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}
