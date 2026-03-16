namespace persist_net_backend.DTOs.MetodoPago
{
    public class UpdateMetodoPagoRequest
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool? Activo { get; set; }
    }
}
