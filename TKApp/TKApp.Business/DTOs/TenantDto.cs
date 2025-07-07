using System;

namespace TKApp.Business.DTOs
{
    public class TenantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
