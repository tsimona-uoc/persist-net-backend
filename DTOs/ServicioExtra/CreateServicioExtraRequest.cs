namespace persist_net_backend.DTOs.ServicioExtra
{
    public class CreateServicioExtraRequest
    {
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public required decimal PrecioBase { get; set; }
    }
}
