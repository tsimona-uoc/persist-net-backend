namespace persist_net_backend.DTOs.Tarifa
{
    public class CreateTarifaRequest
    {
        public required int TemporadaId { get; set; }
        public required decimal PrecioNoche { get; set; }
    }
}
