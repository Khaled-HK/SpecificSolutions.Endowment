using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.Branchs;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class BranchConfig : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.Property(b => b.Name).IsRequired().HasMaxLength(200);
            builder.Property(b => b.Address).IsRequired().HasMaxLength(500);
            builder.Property(b => b.ContactNumber).HasMaxLength(20);

            // relationships with bank (one to many) 
            builder.HasOne(b => b.Bank)
                .WithMany(b => b.Branches)
                .HasForeignKey(b => b.BankId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}