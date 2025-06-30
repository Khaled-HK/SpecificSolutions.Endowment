using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.Products;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).HasMaxLength(500);

        }
    }
}