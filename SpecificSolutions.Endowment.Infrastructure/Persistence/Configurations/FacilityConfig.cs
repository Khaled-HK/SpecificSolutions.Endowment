using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.Facilities;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class FacilityConfig : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Name).IsRequired().HasMaxLength(200);
            builder.Property(f => f.Location).IsRequired().HasMaxLength(200);
            builder.Property(f => f.ContactInfo).IsRequired().HasMaxLength(100);
            builder.Property(f => f.Capacity).IsRequired();
            builder.Property(f => f.Status).IsRequired().HasMaxLength(50);
        }
    }
} 