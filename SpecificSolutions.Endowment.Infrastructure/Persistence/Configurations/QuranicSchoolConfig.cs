using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.QuranicSchools;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class QuranicSchoolConfig : IEntityTypeConfiguration<QuranicSchool>
    {
        public void Configure(EntityTypeBuilder<QuranicSchool> builder)
        {
            builder.HasKey(qs => qs.Id);
        }
    }
}