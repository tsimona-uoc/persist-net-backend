using persist_net_backend.DTOs.ServicioExtra;

namespace persist_net_backend.DTOs.ConsumoExtra
{
    public class ConsumoExtraResponse
    {
        public int Id { get; set; }
        public int EstanciaId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public DateTime Fecha { get; set; }
        public required ServicioExtraResponse ServicioExtra { get; set; }
    }
}
