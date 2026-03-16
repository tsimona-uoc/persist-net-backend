namespace persist_net_backend.DTOs.Pago
{
    public class UpdatePagoRequest
    {
        public int? FacturaId { get; set; }
        public int? MetodoPagoId { get; set; }
        public decimal? Importe { get; set; }
        public DateTime? FechaPago { get; set; }
    }
}
