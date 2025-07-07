using System;
using System.ComponentModel.DataAnnotations;
using TKApp.Core.Enums;
using TKApp.Core.Interfaces;

namespace TKApp.Core.Models
{
    public abstract class BaseEntity : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
