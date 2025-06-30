using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.BuildingDetails;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class BuildingDetailConfig : IEntityTypeConfiguration<BuildingDetail>
    {
        public void Configure(EntityTypeBuilder<BuildingDetail> builder)
        {
            builder.Property(bd => bd.Name).IsRequired().HasMaxLength(200);
            builder.Property(bd => bd.Floors).IsRequired();
            // configure enum BuildingCategory as string
            builder.Property(bd => bd.BuildingCategory)
                .HasConversion<string>();

            // add relationship to Building
            builder.HasOne(bd => bd.Building)
                .WithMany(db => db.BuildingDetails)
                .HasForeignKey(bd => bd.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}