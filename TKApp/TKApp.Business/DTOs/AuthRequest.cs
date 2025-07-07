using System.ComponentModel.DataAnnotations;

namespace TKApp.Business.DTOs
{
    public class AuthRequest
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
