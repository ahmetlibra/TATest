using System.Threading.Tasks;
using TKApp.Business.DTOs;

namespace TKApp.Business.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(AuthRequest request);
        Task<AuthResponse> RefreshTokenAsync(string token, string refreshToken);
        Task<bool> RevokeTokenAsync(string token);
    }
}
