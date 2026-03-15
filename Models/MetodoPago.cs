using System.ComponentModel.DataAnnotations;

namespace persist_net_backend.Models
{
    public class MetodoPago
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        // Relations
        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }

    public enum MetodoPagoEnum
    {
        EFECTIVO = 1,
        TARJETA = 2,
        TRANSFERENCIA = 3
    }
}

