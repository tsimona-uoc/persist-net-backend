using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace persist_net_backend.Models
{
    public class Factura
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Estancia")]
        public int EstanciaId { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Descuento { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Total { get; set; }

        [Required]
        public DateTime FechaEmision { get; set; } = DateTime.Now;

        [Required]
        public bool Pagada { get; set; } = false;

        [Required]
        [StringLength(255)]
        public string LastModifiedBy { get; set; } = string.Empty;

        [Required]
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        // Relaciones
        public virtual Estancia? Estancia { get; set; }
        public virtual Cliente? Cliente { get; set; }
        public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
