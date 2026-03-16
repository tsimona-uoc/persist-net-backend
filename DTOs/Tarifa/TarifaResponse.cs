using persist_net_backend.DTOs.Temporada;

namespace persist_net_backend.DTOs.Tarifa
{
    public class TarifaResponse
    {
        public int Id { get; set; }
        public required TemporadaResponse Temporada { get; set; }
        public decimal PrecioNoche { get; set; }
    }
}
