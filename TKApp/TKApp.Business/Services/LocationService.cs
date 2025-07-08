using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TKApp.Business.DTOs;
using TKApp.Business.Interfaces;
using TKApp.Core.Interfaces;
using TKApp.Data.Contexts;
using TKApp.Entities.Models;
using TKApp.Shared.Interfaces;

namespace TKApp.Business.Services
{
    public class LocationService : ILocationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITenantProvider _tenantProvider;
        private readonly IVehicleService _vehicleService;

        public LocationService(
            AppDbContext context,
            IMapper mapper,
            ITenantProvider tenantProvider,
            IVehicleService vehicleService)
        {
            _context = context;
            _mapper = mapper;
            _tenantProvider = tenantProvider;
            _vehicleService = vehicleService;
        }

        public async Task UpdateVehicleLocationAsync(int vehicleId, decimal latitude, decimal longitude)
        {
            // Update the vehicle's current location
            await _vehicleService.UpdateLocationAsync(vehicleId, latitude, longitude);
            
            // Add to location history
            var locationHistory = new LocationHistory
            {
                VehicleId = vehicleId,
                Latitude = (double)latitude,
                Longitude = (double)longitude,
                Timestamp = DateTime.UtcNow,
                TenantId = _tenantProvider.GetTenantId() ?? throw new InvalidOperationException("Tenant ID is required")
            };

            _context.LocationHistories.Add(locationHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<LocationDto> GetVehicleLocationAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == vehicleId && !v.IsDeleted);

            if (vehicle == null)
                throw new KeyNotFoundException("Vehicle not found");

            return new LocationDto
            {
                VehicleId = vehicle.Id,
                Latitude = vehicle.LastLatitude ?? 0,
                Longitude = vehicle.LastLongitude ?? 0,
                Timestamp = vehicle.LastLocationUpdate ?? DateTime.UtcNow
            };
        }

        public async Task<IEnumerable<LocationHistoryDto>> GetVehicleLocationHistoryAsync(int vehicleId, int count = 100)
        {
            var history = await _context.LocationHistories
                .Where(lh => lh.VehicleId == vehicleId)
                .OrderByDescending(lh => lh.Timestamp)
                .Take(count)
                .ToListAsync();

            return _mapper.Map<IEnumerable<LocationHistoryDto>>(history);
        }

        public async Task UpdateVehicleLocationsBatchAsync(IDictionary<int, LocationUpdateDto> locationUpdates)
        {
            var vehicleIds = locationUpdates.Keys.ToList();
            var vehicles = await _context.Vehicles
                .Where(v => vehicleIds.Contains(v.Id) && !v.IsDeleted)
                .ToListAsync();

            var now = DateTime.UtcNow;
            var tenantId = _tenantProvider.GetTenantId() ?? throw new InvalidOperationException("Tenant ID is required");
            var locationHistories = new List<LocationHistory>();

            foreach (var vehicle in vehicles)
            {
                if (locationUpdates.TryGetValue(vehicle.Id, out var update))
                {
                    // Update vehicle's current location
                    vehicle.LastLatitude = update.Latitude;
                    vehicle.LastLongitude = update.Longitude;
                    vehicle.LastLocationUpdate = now;

                    // Add to location history
                    locationHistories.Add(new LocationHistory
                    {
                        VehicleId = vehicle.Id,
                        Latitude = update.Latitude,
                        Longitude = update.Longitude,
                        Address = update.Address,
                        Timestamp = now,
                        TenantId = tenantId
                    });
                }
            }

            // Save all changes in a transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.SaveChangesAsync();
                await _context.LocationHistories.AddRangeAsync(locationHistories);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
