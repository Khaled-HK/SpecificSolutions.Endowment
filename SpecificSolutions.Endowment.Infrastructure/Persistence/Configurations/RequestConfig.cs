using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class RequestConfig : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            // Specify the table name
            //builder.ToTable("Requests");

            // Specify the primary key
            builder.HasKey(r => r.Id);

            // Configure properties
            builder.Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Description)
                .HasMaxLength(500);

            builder.Property(r => r.CreatedDate)
                .IsRequired();

            builder.Property(r => r.ReferenceNumber)
                .IsRequired()
                .HasMaxLength(50);

            // relationship with Decision
            //builder.HasOne(r => r.Decision)
            //    .WithMany()
            //    .HasForeignKey(r => r.DecisionId);
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
