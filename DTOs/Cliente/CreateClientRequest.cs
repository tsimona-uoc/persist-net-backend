using System.ComponentModel.DataAnnotations;

namespace persist_net_backend.DTOs.Cliente
{
    public class CreateClientRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 255 caracteres.")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 255 caracteres.")]
        public required string Apellido { get; set; }

        [Required(ErrorMessage = "El documento es obligatorio.")]
        public required string Documentacion { get; set; }
        
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20, ErrorMessage = "El teléfono debe tener hasta 20 caracteres.")]
        public required string Telefono { get; set; }
        
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no es válido.")]
        public required string Email { get; set; }

        [StringLength(255, ErrorMessage = "La ciudad no puede exceder los 255 caracteres.")]
        public string? Ciudad { get; set; }

        [StringLength(500, ErrorMessage = "La dirección no puede exceder los 500 caracteres.")]
        public string? Direccion { get; set; }

        public bool Vip { get; set; } = false;
    }
}