namespace persist_net_backend.DTOs.Regimen
{
    public class UpdateRegimenRequest
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool? Activo { get; set; }
    }
}
