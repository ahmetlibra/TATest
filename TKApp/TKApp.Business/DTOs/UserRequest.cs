using System.ComponentModel.DataAnnotations;
using TKApp.Core.Enums;

namespace TKApp.Business.DTOs
{
    public class UserRequest
    {
        [Required, MaxLength(50)]
        public string Username { get; set; }
        
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }
        
        [Required, MinLength(6)]
        public string Password { get; set; }
        
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [MaxLength(50)]
        public string LastName { get; set; }
        
        [Required]
        public UserRole Role { get; set; }
        
        public bool IsShortDistanceAllowed { get; set; }
        public bool IsLongDistanceAllowed { get; set; }
    }
}
