using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TKApp.Core.Models;
using TKApp.Core.Models;

namespace TKApp.Entities.Models
{
    [Table("SystemEnvironments")]
    public class SystemEnvironment : BaseEntity
    {
        [MaxLength(100)]
        public string MainLocation { get; set; }
        
        public double? HalfDiameter { get; set; }
        
        [MaxLength(100)]
        public string TrioApiBaseUrl { get; set; }
        
        [MaxLength(100)]
        public string TrioApiUsername { get; set; }
        
        [MaxLength(100)]
        public string TrioApiPassword { get; set; }
        
        [MaxLength(100)]
        public string TrioApiToken { get; set; }
        
        public DateTime? TrioApiTokenExpiry { get; set; }
        
        [MaxLength(500)]
        public string TrioApiEndpoints { get; set; } // JSON serialized endpoints
    }
}
