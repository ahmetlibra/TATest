using System.Collections.Generic;
using System.Threading.Tasks;
using TKApp.Business.DTOs;
using TKApp.Core.Enums;

namespace TKApp.Business.Interfaces
{
    public interface IVehicleService
    {
        Task<VehicleDto> GetByIdAsync(int id);
        Task<IEnumerable<VehicleDto>> GetAllAsync();
        Task<VehicleDto> CreateAsync(VehicleRequest request);
        Task UpdateAsync(int id, VehicleRequest request);
        Task DeleteAsync(int id);
        Task UpdateLocationAsync(int vehicleId, decimal latitude, decimal longitude);
        Task<IEnumerable<VehicleDto>> GetByTypeAsync(VehicleType type);
        Task<IEnumerable<VehicleDto>> GetByUserIdAsync(int userId);
    }
}
