using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.Accounts;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.MotherName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.BirthDate)
                .IsRequired();

            builder.Property(a => a.Gender)
                .HasConversion<string>();

            builder.Property(a => a.Barcode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.Status)
                .HasConversion<string>();

            builder.Property(a => a.LockerFileNumber)
                .IsRequired();

            builder.Property(a => a.SocialStatus)
                .IsRequired();

            builder.Property(a => a.BookNumber)
                .IsRequired();

            builder.Property(a => a.PaperNumber)
                .IsRequired();

            builder.Property(a => a.RegistrationNumber)
                .IsRequired();

            builder.Property(a => a.AccountNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.Type)
                .HasConversion<string>();

            builder.Property(a => a.LookOver)
                .IsRequired();

            builder.Property(a => a.Note)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.NID)
                .IsRequired();

            builder.Property(a => a.IsActive)
                .IsRequired();

            builder.Property(a => a.Balance)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        }
    }
}