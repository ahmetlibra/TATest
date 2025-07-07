using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TKApp.Core.Interfaces;
using TKApp.Core.Models;
using TKApp.Data.Repositories;
using TKApp.Entities.Models;
using TKApp.Shared.Interfaces;

namespace TKApp.Data.Contexts
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly ITenantProvider _tenantProvider;
        private int? _tenantId;

        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantProvider tenantProvider) 
            : base(options)
        {
            _tenantProvider = tenantProvider;
            _tenantId = _tenantProvider.GetTenantId();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<SystemEnvironment> SystemEnvironments { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Apply entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            
            // Apply global query filter for multi-tenancy
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(IEntity).IsAssignableFrom(e.ClrType)))
            {
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                var property = System.Linq.Expressions.Expression.Property(parameter, "TenantId");
                var tenantId = System.Linq.Expressions.Expression.Constant(_tenantId);
                var equal = System.Linq.Expressions.Expression.Equal(property, tenantId);
                var lambda = System.Linq.Expressions.Expression.Lambda(equal, parameter);
                
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        public override int SaveChanges()
        {
            SetAuditInfo();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditInfo();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetAuditInfo()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && 
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            var currentUserId = _tenantProvider.GetCurrentUserId();
            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = now;
                    entity.CreatedBy = currentUserId;
                    entity.TenantId = _tenantId ?? 0;
                }
                else
                {
                    entity.UpdatedAt = now;
                    entity.UpdatedBy = currentUserId;
                    Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                    Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    Entry(entity).Property(x => x.TenantId).IsModified = false;
                }
            }
        }

        public IRepository<T> GetRepository<T>() where T : class, IEntity
        {
            return new Repository<T>(this);
        }

     
    }
}
