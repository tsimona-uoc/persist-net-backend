namespace persist_net_backend.DTOs.ConsumoExtra
{
    public class CreateConsumoExtraRequest
    {
        public required int EstanciaId { get; set; }
        public required int ServicioExtraId { get; set; }
        public required int Cantidad { get; set; }
        public required decimal PrecioUnitario { get; set; }
        public required DateTime Fecha { get; set; }
    }
}