using System.Collections.Generic;
using System.Threading.Tasks;
using TKApp.Business.DTOs;

namespace TKApp.Business.Interfaces
{
    public interface ITenantService
    {
        Task<TenantDto> GetByIdAsync(int id);
        Task<IEnumerable<TenantDto>> GetAllAsync();
        Task<TenantDto> CreateAsync(TenantRequest request);
        Task UpdateAsync(int id, TenantRequest request);
        Task DeleteAsync(int id);
    }
}
