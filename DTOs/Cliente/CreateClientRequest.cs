namespace persist_net_backend.DTOs.Cliente
{
    public class CreateClientRequest
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Documentacion { get; set; }
        public required string Telefono { get; set; }
        public required string Email { get; set; }
    }
}