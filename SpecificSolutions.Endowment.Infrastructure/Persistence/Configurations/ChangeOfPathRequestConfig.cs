using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.ChangeOfPathRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class ChangeOfPathRequestConfig : IEntityTypeConfiguration<ChangeOfPathRequest>
    {
        public void Configure(EntityTypeBuilder<ChangeOfPathRequest> builder)
        {
            builder.HasKey(cpr => cpr.Id);
            builder.Property(cpr => cpr.CurrentType).IsRequired().HasMaxLength(100);
            builder.Property(cpr => cpr.NewType).IsRequired().HasMaxLength(100);
            builder.Property(cpr => cpr.Reason).HasMaxLength(500);

            // Relationships with Request
            builder.HasOne(cpr => cpr.Request)
                .WithOne(r => r.ChangeOfPathRequest)
                .HasForeignKey<ChangeOfPathRequest>(cpr => cpr.RequestId)
                .IsRequired();

        }
    }
}