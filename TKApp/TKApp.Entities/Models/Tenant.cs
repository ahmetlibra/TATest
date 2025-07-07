using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TKApp.Core.Enums;
using TKApp.Core.Models;

namespace TKApp.Entities.Models
{
    [Table("Tenants")]
    public class Tenant : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        
        public Status Status { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}
