using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.EndowmentExpenditureChangeRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class ExpenditureChangeRequestConfig : IEntityTypeConfiguration<ExpenditureChangeRequest>
    {
        public void Configure(EntityTypeBuilder<ExpenditureChangeRequest> builder)
        {
            builder.HasKey(eecr => eecr.Id);
            builder.Property(eecr => eecr.Reason).HasMaxLength(500);

            builder.HasOne(eecr => eecr.Request)
                .WithOne(r => r.ExpenditureChangeRequest)
                .HasForeignKey<ExpenditureChangeRequest>(eecr => eecr.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // relationship with Branch entity 
            builder.HasOne(eecr => eecr.CurrentExpenditureBranch)
                .WithMany(eecr => eecr.CurrentExpenditureRequests)
                .HasForeignKey(eecr => eecr.CurrentExpenditureBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // relationship with Branch entity 
            builder.HasOne(eecr => eecr.NewExpenditureBranch)
                .WithMany(eecr => eecr.NewExpenditureRequests)
                .HasForeignKey(eecr => eecr.NewExpenditureBranchId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}