namespace persist_net_backend.DTOs.Regimen
{
    public class CreateRegimenRequest
    {
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; } = true;
    }
}
