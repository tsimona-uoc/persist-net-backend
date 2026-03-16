using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace persist_net_backend.Models
{
    public class Reserva
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        [Required]
        [ForeignKey("Habitacion")]
        public int HabitacionId { get; set; }

        [Required]
        public DateOnly FechaEntrada { get; set; }

        [Required]
        public DateOnly FechaSalida { get; set; }

        [Required]
        [ForeignKey("Regimen")]
        public int RegimenId { get; set; }

        [Required]
        [ForeignKey("EstadoReserva")]
        public int EstadoReservaId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PrecioActual { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        [StringLength(255)]
        public string LastModifiedBy { get; set; } = string.Empty;

        [Required]
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        // Relaciones
        public virtual Cliente? Cliente { get; set; }
        public virtual Habitacion? Habitacion { get; set; }
        public virtual Regimen? Regimen { get; set; }
        public virtual EstadoReserva? EstadoReserva { get; set; }
        public virtual ICollection<Estancia> Estancias { get; set; } = new List<Estancia>();
    }
}
