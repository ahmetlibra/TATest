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

namespace TKApp.Business.Services
{
    public class TenantService : ITenantService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TenantService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TenantDto> GetByIdAsync(int id)
        {
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.Id == id && t.Status != Status.Deleted);
                
            if (tenant == null)
                throw new KeyNotFoundException("Tenant not found");
                
            return _mapper.Map<TenantDto>(tenant);
        }

        public async Task<IEnumerable<TenantDto>> GetAllAsync()
        {
            var tenants = await _context.Tenants
                .Where(t => t.Status != Status.Deleted)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<TenantDto>>(tenants);
        }

        public async Task<TenantDto> CreateAsync(TenantRequest request)
        {
            if (await _context.Tenants.AnyAsync(t => t.Name == request.Name && t.Status != Status.Deleted))
                throw new Exception("A tenant with this name already exists");

            var tenant = new Tenant
            {
                Name = request.Name,
                Status = Status.Active,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();

            return _mapper.Map<TenantDto>(tenant);
        }

        public async Task UpdateAsync(int id, TenantRequest request)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null || tenant.Status == Status.Deleted)
                throw new KeyNotFoundException("Tenant not found");

            if (await _context.Tenants.AnyAsync(t => t.Id != id && t.Name == request.Name && t.Status != Status.Deleted))
                throw new Exception("A tenant with this name already exists");

            tenant.Name = request.Name;
            tenant.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant != null && tenant.Status != Status.Deleted)
            {
                // Soft delete
                tenant.Status = Status.Deleted;
                tenant.UpdatedAt = DateTime.UtcNow;
                
                // Optionally, deactivate all users and resources associated with this tenant
                await DeactivateTenantResources(id);
                
                await _context.SaveChangesAsync();
            }
        }

        private async Task DeactivateTenantResources(int tenantId)
        {
            // Deactivate all users for this tenant
            var users = await _context.Users
                .Where(u => u.TenantId == tenantId && u.Status != Status.Deleted)
                .ToListAsync();

            foreach (var user in users)
            {
                user.Status = Status.Suspend;
                user.UpdatedAt = DateTime.UtcNow;
            }

            // Deactivate all vehicles for this tenant
            var vehicles = await _context.Vehicles
                .Where(v => v.TenantId == tenantId && v.Status != Status.Deleted)
                .ToListAsync();

            foreach (var vehicle in vehicles)
            {
                vehicle.Status = Status.Suspend;
                vehicle.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
