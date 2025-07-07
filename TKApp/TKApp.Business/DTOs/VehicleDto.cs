using System;
using TKApp.Core.Enums;

namespace TKApp.Business.DTOs
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PlateNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int? ModelYear { get; set; }
        public VehicleType Type { get; set; }
        public string Company { get; set; }
        public bool IsShortDistanceAllowed { get; set; }
        public bool IsLongDistanceAllowed { get; set; }
        public DateTime? LastLocationUpdate { get; set; }
        public double? LastLatitude { get; set; }
        public double? LastLongitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
