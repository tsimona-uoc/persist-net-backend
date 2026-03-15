using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace persist_net_backend.Models
{
    public class Estancia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Reserva")]
        public int ReservaId { get; set; }

        [Required]
        public DateTime FechaCheckIn { get; set; }

        public DateTime? FechaCheckOut { get; set; }

        [Required]
        [ForeignKey("EstadoEstancia")]
        public int EstadoEstanciaId { get; set; }

        [Required]
        [StringLength(255)]
        public string LastModifiedBy { get; set; } = string.Empty;

        [Required]
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        // Relaciones
        public Reserva? Reserva { get; set; }
        public EstadoEstancia? EstadoEstancia { get; set; }
        public ICollection<ConsumoExtra> ConsumosExtra { get; set; } = new List<ConsumoExtra>();
        public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
    }
}
