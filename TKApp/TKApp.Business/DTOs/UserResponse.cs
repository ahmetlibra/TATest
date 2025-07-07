using System;
using TKApp.Core.Enums;

namespace TKApp.Business.DTOs
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        public bool IsShortDistanceAllowed { get; set; }
        public bool IsLongDistanceAllowed { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedUntil { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
