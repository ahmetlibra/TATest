namespace TKApp.Business.DTOs
{
    public class LocationDto
    {
        public int VehicleId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public System.DateTime Timestamp { get; set; }
        public string Address { get; set; }
    }
}
