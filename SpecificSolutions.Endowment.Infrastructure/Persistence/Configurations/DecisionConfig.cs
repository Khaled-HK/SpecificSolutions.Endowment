using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.Decisions;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class DecisionConfig : IEntityTypeConfiguration<Decision>
    {
        public void Configure(EntityTypeBuilder<Decision> builder)
        {
            //builder.ToTable("Decisions");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(d => d.CreatedDate)
                .IsRequired();

            builder.Property(d => d.ReferenceNumber)
                .IsRequired()
                .HasMaxLength(50);

            //// Configure the relationship between Decision and Appuser
            //builder.HasOne(d => d.AppUser)
            //    .WithMany(d => d.Decisions)
            //    .HasForeignKey(d => d.AppUserId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}