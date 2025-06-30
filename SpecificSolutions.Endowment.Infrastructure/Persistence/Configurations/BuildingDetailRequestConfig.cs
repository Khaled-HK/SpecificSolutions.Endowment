using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.BuildingDetailRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class BuildingDetailRequestConfig : IEntityTypeConfiguration<BuildingDetailRequest>
    {
        public void Configure(EntityTypeBuilder<BuildingDetailRequest> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.RequestDetails).IsRequired().HasMaxLength(1000);
            builder.Property(b => b.RequestDate).IsRequired();

            builder.HasOne(b => b.BuildingDetail)
                .WithMany(b => b.BuildingDetailRequests)
                .HasForeignKey(b => b.BuildingDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Request)
                .WithMany(b => b.BuildingDetailRequests)
                .HasForeignKey(b => b.RequestId);

        }
    }
}