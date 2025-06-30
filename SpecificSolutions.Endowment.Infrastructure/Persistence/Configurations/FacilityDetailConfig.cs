using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.FacilityDetails;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class FacilityDetailConfig : IEntityTypeConfiguration<FacilityDetail>
    {
        public void Configure(EntityTypeBuilder<FacilityDetail> builder)
        {
            builder.HasKey(md => md.Id);

            // add relationship to Product 
            builder.HasOne(bd => bd.Product)
                .WithMany(db => db.FacilityDetails)
                .HasForeignKey(bd => bd.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // add relationship with BuildingDetail
            builder.HasOne(bd => bd.BuildingDetail)
                .WithMany(bd => bd.FacilityDetails)
                .HasForeignKey(bd => bd.BuildingDetailId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}