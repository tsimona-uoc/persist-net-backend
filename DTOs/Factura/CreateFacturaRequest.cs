using persist_net_backend.Models;

namespace persist_net_backend.DTOs.Factura
{
    public class CreateFacturaRequest
    {
        public required int EstanciaId { get; set; }
        public required int ClienteId { get; set; }
        public decimal? Descuento { get; set; }
        public required decimal Total { get; set; }
        public DateTime? FechaEmision { get; set; }
        public required bool Pagada { get; set; }
    }
}