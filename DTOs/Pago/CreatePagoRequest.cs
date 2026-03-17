namespace persist_net_backend.DTOs.Pago
{
    public class CreatePagoRequest
    {
        public required int FacturaId { get; set; }
        public required int MetodoPagoId { get; set; }
        public required decimal Importe { get; set; }
        public DateTime? FechaPago { get; set; }
    }
}
