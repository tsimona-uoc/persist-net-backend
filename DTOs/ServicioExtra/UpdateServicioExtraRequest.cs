namespace persist_net_backend.DTOs.ServicioExtra
{
    public class UpdateServicioExtraRequest
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? PrecioBase { get; set; }
    }
}
