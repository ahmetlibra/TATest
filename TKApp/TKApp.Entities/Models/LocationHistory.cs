using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TKApp.Core.Interfaces;
using TKApp.Core.Models;

namespace TKApp.Entities.Models
{
    [Table("LocationHistories")]
    public class LocationHistory : IEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int VehicleId { get; set; }
        
        [Required]
        public double Latitude { get; set; }
        
        [Required]
        public double Longitude { get; set; }
        
        public string Address { get; set; }
        
        [Required]
        public DateTime Timestamp { get; set; }
        
        [Required]
        public int TenantId { get; set; }
        
        // Navigation property
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }
    }
}
