using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.NeedsRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class NeedsRequestConfig : IEntityTypeConfiguration<NeedsRequest>
    {
        public void Configure(EntityTypeBuilder<NeedsRequest> builder)
        {
            builder.HasKey(nr => nr.Id);

            //relationship with Request entity (one-to-one)
            builder.HasOne(nr => nr.Request)
                .WithOne(r => r.NeedsRequest)
                .HasForeignKey<NeedsRequest>(nr => nr.RequestId)
                .IsRequired();
        }
    }
}