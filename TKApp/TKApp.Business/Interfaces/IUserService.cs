using System.Collections.Generic;
using System.Threading.Tasks;
using TKApp.Business.DTOs;
using TKApp.Core.Enums;
using TKApp.Entities.Models;

namespace TKApp.Business.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponse> AuthenticateAsync(AuthRequest request);
        Task<UserResponse> GetByIdAsync(int id);
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse> CreateAsync(UserRequest request);
        Task UpdateAsync(int id, UserRequest request);
        Task DeleteAsync(int id);
        Task ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> CheckPasswordAsync(int userId, string password);
        Task<UserRole[]> GetUserRolesAsync(int userId);
        Task AddToRoleAsync(int userId, UserRole role);
        Task RemoveFromRoleAsync(int userId, UserRole role);
    }
}
