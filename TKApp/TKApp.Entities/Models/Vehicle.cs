using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TKApp.Core.Enums;
using TKApp.Core.Models;

namespace TKApp.Entities.Models
{
    [Table("Vehicles")]
    public class Vehicle : BaseEntity
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
        public VehicleType Type { get; set; }
        
        [MaxLength(100)]
        public string Company { get; set; }
        
        public bool IsShortDistanceAllowed { get; set; }
        public bool IsLongDistanceAllowed { get; set; }
        public DateTime? LastLocationUpdate { get; set; }
        public double? LastLatitude { get; set; }
        public double? LastLongitude { get; set; }
        
        // Navigation property
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
