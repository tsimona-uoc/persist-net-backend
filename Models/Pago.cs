using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace persist_net_backend.Models
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Factura")]
        public int FacturaId { get; set; }

        [Required]
        [ForeignKey("MetodoPago")]
        public int MetodoPagoId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Importe { get; set; }

        [Required]
        public DateTime FechaPago { get; set; } = DateTime.Now;

        [Required]
        [StringLength(255)]
        public string LastModifiedBy { get; set; } = string.Empty;

        [Required]
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        // Relaciones
        public Factura? Factura { get; set; }
        public MetodoPago? MetodoPago { get; set; }
    }
}
