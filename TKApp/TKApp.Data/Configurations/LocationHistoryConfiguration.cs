using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TKApp.Entities.Models;

namespace TKApp.Data.Configurations
{
    public class LocationHistoryConfiguration : IEntityTypeConfiguration<LocationHistory>
    {
        public void Configure(EntityTypeBuilder<LocationHistory> builder)
        {
            builder.HasKey(lh => lh.Id);
            
            builder.Property(lh => lh.Latitude).IsRequired();
            builder.Property(lh => lh.Longitude).IsRequired();
            builder.Property(lh => lh.Timestamp).IsRequired();
            
            // Indexes
            builder.HasIndex(lh => lh.VehicleId);
            builder.HasIndex(lh => lh.Timestamp);
            
            // Relationships
            builder.HasOne(lh => lh.Vehicle)
                .WithMany()
                .HasForeignKey(lh => lh.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
