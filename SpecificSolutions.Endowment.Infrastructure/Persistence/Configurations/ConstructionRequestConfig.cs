using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.ConstructionRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class ConstructionRequestConfig : IEntityTypeConfiguration<ConstructionRequest>
    {
        public void Configure(EntityTypeBuilder<ConstructionRequest> builder)
        {
            builder.HasKey(cr => cr.Id);
            builder.Property(cr => cr.ProposedLocation).IsRequired().HasMaxLength(500);
            builder.Property(cr => cr.ProposedArea).HasColumnType("decimal(18,2)");
            builder.Property(cr => cr.EstimatedCost).HasColumnType("decimal(18,2)");
            builder.Property(cr => cr.ContractorName).HasMaxLength(200);

            builder.HasOne(cr => cr.Request)
                .WithOne(r => r.ConstructionRequest)
                .HasForeignKey<ConstructionRequest>(cr => cr.RequestId)
                .IsRequired();
        }
    }
}