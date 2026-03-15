using System.ComponentModel.DataAnnotations;

namespace persist_net_backend.Models
{
    public class Regimen
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
        public ICollection<Tarifa> Tarifas { get; set; } = new List<Tarifa>();
    }

    public enum RegimenEnum
    {
        AD = 1,  // Alojamiento y Desayuno
        MP = 2,  // Media Pensión
        PC = 3,  // Pensión Completa
        TI = 4   // Todo Incluido
    }
}

