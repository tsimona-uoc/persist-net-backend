namespace persist_net_backend.DTOs.Cliente
{
    public class UpdateClientRequest
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Documentacion { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Ciudad { get; set; }
        public string? Direccion { get; set; }
        public bool? Vip { get; set; }
    }
}