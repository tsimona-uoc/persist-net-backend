namespace persist_net_backend.DTOs.Factura
{
    public class FacturaResponse
    {
        public int Id { get; set; }
        public int EstanciaId { get; set; }
        public int ClienteId { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaEmision { get; set; }
        public bool Pagada { get; set; }
    }
}
