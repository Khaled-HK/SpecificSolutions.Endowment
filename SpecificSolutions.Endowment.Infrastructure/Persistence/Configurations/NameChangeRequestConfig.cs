using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.NameChangeRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class NameChangeRequestConfig : IEntityTypeConfiguration<NameChangeRequest>
    {
        public void Configure(EntityTypeBuilder<NameChangeRequest> builder)
        {
            builder.HasKey(ncr => ncr.Id);
            builder.Property(ncr => ncr.CurrentName).IsRequired().HasMaxLength(200);
            builder.Property(ncr => ncr.NewName).IsRequired().HasMaxLength(200);
            builder.Property(ncr => ncr.Reason).HasMaxLength(500);

            builder.HasOne(ncr => ncr.Request)
                .WithOne(r => r.NameChangeRequest)
                .HasForeignKey<NameChangeRequest>(ncr => ncr.RequestId)
                .IsRequired();
        }
    }
}