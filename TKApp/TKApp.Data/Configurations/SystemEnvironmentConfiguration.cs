using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TKApp.Entities.Models;

namespace TKApp.Data.Configurations
{
    public class SystemEnvironmentConfiguration : IEntityTypeConfiguration<SystemEnvironment>
    {
        public void Configure(EntityTypeBuilder<SystemEnvironment> builder)
        {
            builder.HasKey(se => se.Id);
            builder.Property(se => se.MainLocation).HasMaxLength(100);
            builder.Property(se => se.TrioApiBaseUrl).HasMaxLength(100);
            builder.Property(se => se.TrioApiUsername).HasMaxLength(100);
            builder.Property(se => se.TrioApiPassword).HasMaxLength(100);
            builder.Property(se => se.TrioApiToken).HasMaxLength(100);
            builder.Property(se => se.TrioApiEndpoints).HasMaxLength(500);
        }
    }
}
