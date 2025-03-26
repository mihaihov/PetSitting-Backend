using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Domain.Entities.Security
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; } = DateTime.Now.AddDays(5);
        public bool IsRevoked { get; set; } = false;
        public bool IsUsed { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string CreatedByIp { get; set; } = string.Empty;

        // Foreign key relationship
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}