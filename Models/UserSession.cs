using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace persist_net_backend.Models
{
    [Index(nameof(UserId), IsUnique = true)]
    public class UserSession
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column(TypeName = "uniqueidentifier")]
        public Guid UserId { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string JwtToken { get; set; } = string.Empty;

        [NotNull]
        [Column(TypeName = "nvarchar(255)")]
        public string LastModifiedBy { get; set; } = string.Empty;

        [NotNull]
        [Column(TypeName = "datetime2")]
        public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}
