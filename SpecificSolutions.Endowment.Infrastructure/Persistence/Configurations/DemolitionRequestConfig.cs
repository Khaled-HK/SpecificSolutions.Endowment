using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.DemolitionRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class DemolitionRequestConfig : IEntityTypeConfiguration<DemolitionRequest>
    {
        public void Configure(EntityTypeBuilder<DemolitionRequest> builder)
        {
            builder.HasKey(drr => drr.Id);
            builder.Property(drr => drr.Reason).IsRequired().HasMaxLength(500);

            builder.HasOne(drr => drr.Request)
                .WithOne(c => c.DemolitionRequest)
                .HasForeignKey<DemolitionRequest>(drr => drr.RequestId)
                .IsRequired();
        }
    }
}