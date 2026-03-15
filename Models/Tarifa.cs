using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace persist_net_backend.Models
{
    public class Tarifa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Temporada")]
        public int TemporadaId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PrecioNoche { get; set; }

        [Required]
        [StringLength(255)]
        public string LastModifiedBy { get; set; } = string.Empty;

        [Required]
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        // Relaciones
        public Temporada? Temporada { get; set; }
    }
}
