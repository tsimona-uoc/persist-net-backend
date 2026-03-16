using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace persist_net_backend.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [NotNull]
        [Column(TypeName = "nvarchar(255)")]
        public string Name { get; set; } = string.Empty;

        [NotNull]
        [Column(TypeName = "nvarchar(255)")]
        public string Surname { get; set; } = string.Empty;

        [NotNull]
        [Column(TypeName = "nvarchar(255)")]
        public string Email { get; set; } = string.Empty;

        [NotNull]
        [Column(TypeName = "nvarchar(255)")]
        public string PasswordHash { get; set; } = string.Empty;

        [ForeignKey("UserRole")]
        public int? UserRoleId { get; set; }

        [NotNull]
        [Column(TypeName = "nvarchar(255)")]
        public string LastModifiedBy { get; set; } = string.Empty;

        [NotNull]
        [Column(TypeName = "datetime2")]
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        public virtual UserRole? UserRole { get; set; }

        /// <summary>
        /// Una colección de sesiones activas para este usuario. Cada sesión representa un token JWT emitido para este usuario.
        /// </summary>
        public virtual ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();
    }
}
