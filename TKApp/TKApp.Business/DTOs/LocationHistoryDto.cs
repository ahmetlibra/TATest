using System;

namespace TKApp.Business.DTOs
{
    public class LocationHistoryDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
        public string Address { get; set; }
    }
}
