using System.ComponentModel.DataAnnotations;
using TKApp.Core.Enums;

namespace TKApp.Business.DTOs
{
    public class VehicleRequest
    {
        [Required]
        public int UserId { get; set; }
        
        [Required, MaxLength(20)]
        public string PlateNumber { get; set; }
        
        [MaxLength(100)]
        public string Brand { get; set; }
        
        [MaxLength(100)]
        public string Model { get; set; }
        
        public int? ModelYear { get; set; }
        
        [Required]
        public VehicleType Type { get; set; }
        
        [MaxLength(100)]
        public string Company { get; set; }
        
        public bool IsShortDistanceAllowed { get; set; }
        public bool IsLongDistanceAllowed { get; set; }
    }
}
