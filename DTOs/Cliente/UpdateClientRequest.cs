namespace persist_net_backend.DTOs.Cliente
{
    public class UpdateClientRequest
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Documentacion { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }
}