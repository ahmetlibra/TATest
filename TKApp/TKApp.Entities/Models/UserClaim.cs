using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TKApp.Core.Models;

namespace TKApp.Entities.Models
{
    [Table("UserClaims")]
    public class UserClaim : BaseEntity
    {
        [Required]
        public int UserId { get; set; }
        
        [Required, MaxLength(100)]
        public string ClaimType { get; set; }
        
        [MaxLength(500)]
        public string ClaimValue { get; set; }
        
        // Navigation property
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
