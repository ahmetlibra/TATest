using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TKApp.Entities.Models;

namespace TKApp.Data.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(v => v.Id);
            builder.Property(v => v.PlateNumber).IsRequired().HasMaxLength(20);
            builder.Property(v => v.Brand).HasMaxLength(100);
            builder.Property(v => v.Model).HasMaxLength(100);
            builder.Property(v => v.Company).HasMaxLength(100);
            builder.Property(v => v.Type).IsRequired();
            
            // Indexes
            builder.HasIndex(v => v.PlateNumber).IsUnique();
            
            // Relationships
            builder.HasOne(v => v.User)
                .WithMany(u => u.Vehicles)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
