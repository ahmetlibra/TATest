using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TKApp.Business.DTOs;
using TKApp.Business.Interfaces;
using TKApp.Core.Enums;
using TKApp.Core.Interfaces;
using TKApp.Data.Contexts;
using TKApp.Entities.Models;
using TKApp.Shared.Interfaces;

namespace TKApp.Business.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITenantProvider _tenantProvider;

        public VehicleService(
            AppDbContext context,
            IMapper mapper,
            ITenantProvider tenantProvider)
        {
            _context = context;
            _mapper = mapper;
            _tenantProvider = tenantProvider;
        }

        public async Task<VehicleDto> GetByIdAsync(int id)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);
                
            return _mapper.Map<VehicleDto>(vehicle);
        }

        public async Task<IEnumerable<VehicleDto>> GetAllAsync()
        {
            var vehicles = await _context.Vehicles
                .Where(v => !v.IsDeleted)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
        }

        public async Task<VehicleDto> CreateAsync(VehicleRequest request)
        {
            if (await _context.Vehicles.AnyAsync(v => v.PlateNumber == request.PlateNumber && !v.IsDeleted))
                throw new Exception("A vehicle with this plate number already exists");

            var vehicle = _mapper.Map<Vehicle>(request);
            vehicle.CreatedAt = DateTime.UtcNow;
            vehicle.TenantId = _tenantProvider.GetTenantId() ?? throw new InvalidOperationException("Tenant ID is required");
            vehicle.Status = Status.Active;

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return _mapper.Map<VehicleDto>(vehicle);
        }

        public async Task UpdateAsync(int id, VehicleRequest request)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);
                
            if (vehicle == null)
                throw new KeyNotFoundException("Vehicle not found");

            if (await _context.Vehicles.AnyAsync(v => v.Id != id && v.PlateNumber == request.PlateNumber && !v.IsDeleted))
                throw new Exception("A vehicle with this plate number already exists");

            _mapper.Map(request, vehicle);
            vehicle.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                vehicle.IsDeleted = true;
                vehicle.Status = Status.Deleted;
                vehicle.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateLocationAsync(int vehicleId, decimal latitude, decimal longitude)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null || vehicle.IsDeleted)
                throw new KeyNotFoundException("Vehicle not found");

            vehicle.LastLatitude = (double)latitude;
            vehicle.LastLongitude = (double)longitude;
            vehicle.LastLocationUpdate = DateTime.UtcNow;

            // In a real application, you would also add this to a location history table
            // and potentially call an external geocoding service to get the address

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<VehicleDto>> GetByTypeAsync(VehicleType type)
        {
            var vehicles = await _context.Vehicles
                .Where(v => v.Type == type && !v.IsDeleted)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
        }

        public async Task<IEnumerable<VehicleDto>> GetByUserIdAsync(int userId)
        {
            var vehicles = await _context.Vehicles
                .Where(v => v.UserId == userId && !v.IsDeleted)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
        }
    }
}
