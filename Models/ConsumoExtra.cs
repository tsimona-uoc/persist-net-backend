using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace persist_net_backend.Models
{
    public class ConsumoExtra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Estancia")]
        public int EstanciaId { get; set; }

        [Required]
        [ForeignKey("ServicioExtra")]
        public int ServicioExtraId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PrecioUnitario { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [StringLength(255)]
        public string LastModifiedBy { get; set; } = string.Empty;

        [Required]
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        // Relaciones
        public virtual Estancia? Estancia { get; set; }
        public virtual ServicioExtra? ServicioExtra { get; set; }
    }
}
