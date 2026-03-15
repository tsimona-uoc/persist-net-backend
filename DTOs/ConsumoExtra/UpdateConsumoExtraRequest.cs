namespace persist_net_backend.DTOs.ConsumoExtra
{
    public class UpdateConsumoExtraRequest
    {
        public int? EstanciaId { get; set; }
        public int? ServicioExtraId { get; set; }
        public int? Cantidad { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public DateTime? Fecha { get; set; }
    }
}