using System.ComponentModel.DataAnnotations;

namespace persist_net_backend.Models
{
    public class TipoHabitacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string LastModifiedBy { get; set; } = string.Empty;

        [Required]
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        // Relaciones
        public ICollection<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();
    }
}
