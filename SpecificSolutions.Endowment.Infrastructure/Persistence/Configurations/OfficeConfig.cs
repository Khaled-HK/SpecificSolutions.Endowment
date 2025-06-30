using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.Offices;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class OfficeConfig : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.Property(b => b.Name).IsRequired().HasMaxLength(200);
            // Other configurations...

            builder.HasOne(b => b.Region)
                .WithMany(r => r.Offices)
                .HasForeignKey(b => b.RegionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}