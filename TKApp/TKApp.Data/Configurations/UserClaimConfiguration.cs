using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TKApp.Entities.Models;

namespace TKApp.Data.Configurations
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.HasKey(uc => uc.Id);
            builder.Property(uc => uc.ClaimType).IsRequired().HasMaxLength(100);
            builder.Property(uc => uc.ClaimValue).HasMaxLength(500);
            
            // Relationships
            builder.HasOne(uc => uc.User)
                .WithMany(u => u.Claims)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Indexes
            builder.HasIndex(uc => new { uc.UserId, uc.ClaimType });
        }
    }
}
