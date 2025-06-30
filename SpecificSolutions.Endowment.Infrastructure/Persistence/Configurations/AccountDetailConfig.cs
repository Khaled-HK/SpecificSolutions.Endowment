using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.AccountDetails;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class AccountDetailConfig : IEntityTypeConfiguration<AccountDetail>
    {
        public void Configure(EntityTypeBuilder<AccountDetail> builder)
        {
            builder.ToTable("AccountDetails");

            builder.HasKey(ad => ad.Id);

            builder.Property(ad => ad.Debtor)
                .IsRequired();

            builder.Property(ad => ad.Creditor)
                .IsRequired();

            builder.Property(ad => ad.Note)
                .HasMaxLength(500);

            builder.Property(ad => ad.CreatedDate)
                .IsRequired();

            builder.Property(ad => ad.OperationType)
                .IsRequired();

            builder.Property(ad => ad.OperationNumber)
                .IsRequired();

            builder.Property(ad => ad.Balance)
                .IsRequired();

            builder.HasOne(ad => ad.Account)
                .WithMany(a => a.AccountDetails)
                .HasForeignKey(ad => ad.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}