namespace persist_net_backend.DTOs.EstadoEstancia
{
    public class UpdateEstadoEstanciaRequest
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool? Activo { get; set; }
    }
}
