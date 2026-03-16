namespace persist_net_backend.DTOs.ServicioExtra
{
    public class ServicioExtraResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioBase { get; set; }
    }
}
