namespace persist_net_backend.DTOs.EstadoEstancia
{
    public class CreateEstadoEstanciaRequest
    {
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; } = true;
    }
}
