using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.MaintenanceRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class MaintenanceRequestConfig : IEntityTypeConfiguration<MaintenanceRequest>
    {
        public void Configure(EntityTypeBuilder<MaintenanceRequest> builder)
        {
            builder.HasKey(mr => mr.Id);
            builder.Property(mr => mr.MaintenanceType).IsRequired().HasMaxLength(50);
            builder.Property(mr => mr.Location).IsRequired().HasMaxLength(500);
            builder.Property(mr => mr.EstimatedCost).HasColumnType("decimal(18,2)");

            //relationship with Request entity (one-to-one)
            builder.HasOne(mr => mr.Request)
                .WithOne(r => r.MaintenanceRequest)
                .HasForeignKey<MaintenanceRequest>(mr => mr.RequestId)
                .IsRequired();
        }
    }
}