using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TKApp.Entities.Models;

namespace TKApp.Data.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Status).IsRequired();
            builder.Property(t => t.CreatedAt).IsRequired();
            
            // Indexes
            builder.HasIndex(t => t.Name).IsUnique();
        }
    }
}
