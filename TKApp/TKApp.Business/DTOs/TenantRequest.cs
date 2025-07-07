using System.ComponentModel.DataAnnotations;

namespace TKApp.Business.DTOs
{
    public class TenantRequest
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
