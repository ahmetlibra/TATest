using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TKApp.Core.Enums;
using TKApp.Core.Models;

namespace TKApp.Entities.Models
{
    [Table("Users")]
    public class User : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Username { get; set; }
        
        [Required, MaxLength(100)]
        public string Email { get; set; }
        
        [Required]
        public byte[] PasswordHash { get; set; }
        
        [Required]
        public byte[] PasswordSalt { get; set; }
        
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [MaxLength(50)]
        public string LastName { get; set; }
        
        public UserRole Role { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int FailedLoginAttempts { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedUntil { get; set; }
        public bool IsShortDistanceAllowed { get; set; }
        public bool IsLongDistanceAllowed { get; set; }
        
        // Navigation properties
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<UserClaim> Claims { get; set; }
        
        public User()
        {
            Vehicles = new HashSet<Vehicle>();
            Claims = new HashSet<UserClaim>();
        }
    }
}
