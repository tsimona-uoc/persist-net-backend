using System.ComponentModel.DataAnnotations;

namespace persist_net_backend.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(255)]
        public string Documentacion { get; set; } = string.Empty;

        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        [StringLength(255)]
        public string LastModifiedBy { get; set; } = string.Empty;

        [Required]
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        // Relaciones
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
        public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
    }
}
