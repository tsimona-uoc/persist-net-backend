using persist_net_backend.DTOs.MetodoPago;

namespace persist_net_backend.DTOs.Pago
{
    public class PagoResponse
    {
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public required MetodoPagoResponse MetodoPago { get; set; }
        public decimal Importe { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
