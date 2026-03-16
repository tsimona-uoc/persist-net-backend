namespace persist_net_backend.DTOs.EstadoEstancia
{
    public class EstadoEstanciaResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
