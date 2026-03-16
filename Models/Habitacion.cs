using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace persist_net_backend.Models
{
    public class Habitacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }

        [Required]
        [ForeignKey("TipoHabitacion")]
        public int TipoHabitacionId { get; set; }

        [Required]
        public int Planta { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        [ForeignKey("EstadoHabitacion")]
        public int EstadoHabitacionId { get; set; }

        [Required]
        [StringLength(255)]
        public string LastModifiedBy { get; set; } = string.Empty;

        [Required]
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        // Relaciones
        public virtual Hotel? Hotel { get; set; }
        public virtual TipoHabitacion? TipoHabitacion { get; set; }
        public virtual EstadoHabitacion? EstadoHabitacion { get; set; }
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
