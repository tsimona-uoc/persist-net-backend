namespace persist_net_backend.DTOs.Tarifa
{
    public class UpdateTarifaRequest
    {
        public int? TemporadaId { get; set; }
        public decimal? PrecioNoche { get; set; }
    }
}
