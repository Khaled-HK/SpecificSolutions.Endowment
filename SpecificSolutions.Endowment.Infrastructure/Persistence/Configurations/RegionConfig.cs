using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.Regions;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class RegionConfig : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Name).IsRequired().HasMaxLength(200);
            builder.Property(r => r.Country).IsRequired().HasMaxLength(100);

            builder.HasOne(r => r.City)
                .WithMany(c => c.Regions)
                .HasForeignKey(r => r.CityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}