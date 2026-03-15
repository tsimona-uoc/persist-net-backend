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
        public RegimenEnum Regimen { get; set; }

        [Required]
        public EstadoReservaEnum Estado { get; set; } = EstadoReservaEnum.CREADA;

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
        public Cliente? Cliente { get; set; }
        public Habitacion? Habitacion { get; set; }
        public ICollection<Estancia> Estancias { get; set; } = new List<Estancia>();
    }
}
