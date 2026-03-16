namespace persist_net_backend.DTOs.MetodoPago
{
    public class CreateMetodoPagoRequest
    {
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; } = true;
    }
}
