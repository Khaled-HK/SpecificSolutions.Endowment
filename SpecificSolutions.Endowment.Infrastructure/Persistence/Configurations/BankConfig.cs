using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.Banks;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class BankConfig : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            builder.Property(b => b.Name).IsRequired().HasMaxLength(200);
            builder.Property(b => b.Address).IsRequired().HasMaxLength(500);
            builder.Property(b => b.ContactNumber).HasMaxLength(20);
        }
    }
}