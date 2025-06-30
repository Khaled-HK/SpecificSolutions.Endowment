using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.Buildings;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class BuildingConfig : IEntityTypeConfiguration<Building>
    {
        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.Property(b => b.Name).IsRequired().HasMaxLength(200);
            // configure enum SourceFunds as string
            builder.Property(b => b.SourceFunds)
                .HasConversion<string>();

            // Other configurations...

            builder.HasOne(b => b.Region)
                .WithMany(r => r.Buildings)
                .HasForeignKey(b => b.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Office)
                .WithMany(o => o.Buildings)
                .HasForeignKey(b => b.OfficeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}