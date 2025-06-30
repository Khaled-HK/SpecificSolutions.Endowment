using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.Mosques;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class MosqueConfig : IEntityTypeConfiguration<Mosque>
    {
        public void Configure(EntityTypeBuilder<Mosque> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.MosqueDefinition)
               .HasConversion<string>();
            builder.Property(m => m.MosqueClassification)
                .HasConversion<string>();

        }
    }
}