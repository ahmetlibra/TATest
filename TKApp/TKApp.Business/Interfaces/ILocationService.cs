using System.Collections.Generic;
using System.Threading.Tasks;
using TKApp.Business.DTOs;

namespace TKApp.Business.Interfaces
{
    public interface ILocationService
    {
        Task UpdateVehicleLocationAsync(int vehicleId, decimal latitude, decimal longitude);
        Task<LocationDto> GetVehicleLocationAsync(int vehicleId);
        Task<IEnumerable<LocationHistoryDto>> GetVehicleLocationHistoryAsync(int vehicleId, int count = 100);
        Task UpdateVehicleLocationsBatchAsync(IDictionary<int, LocationUpdateDto> locationUpdates);
    }
}
