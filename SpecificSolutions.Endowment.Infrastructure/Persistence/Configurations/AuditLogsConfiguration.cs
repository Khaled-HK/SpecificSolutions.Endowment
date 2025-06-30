using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpecificSolutions.Endowment.Core.Entities.AuditLogs;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Configurations
{
    public class AuditLogsConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}
