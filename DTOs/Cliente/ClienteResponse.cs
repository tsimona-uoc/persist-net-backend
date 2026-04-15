namespace persist_net_backend.DTOs.Cliente
{
    public class ClienteResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Documentacion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public bool Vip { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
