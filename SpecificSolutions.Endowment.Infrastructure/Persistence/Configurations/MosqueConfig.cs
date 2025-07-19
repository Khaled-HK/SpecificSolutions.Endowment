using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.Mosques;
using SpecificSolutions.Endowment.Core.Entities.Buildings;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class MosqueConfig : IEntityTypeConfiguration<Mosque>
    {
        public void Configure(EntityTypeBuilder<Mosque> builder)
        {
            builder.HasKey(m => m.Id);
            
            // Configure enum conversions
            builder.Property(m => m.MosqueDefinition)
               .HasConversion<string>();
            builder.Property(m => m.MosqueClassification)
                .HasConversion<string>();

            // Configure relationship with Building
            builder.HasOne(m => m.Building)
                .WithOne()
                .HasForeignKey<Mosque>(m => m.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}