using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Core.Entities.AuditLogs;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.AuditLogs
{
    public class AuditLogsRepository : Repository<AuditLog>, IAuditLogsRepository
    {
        public AuditLogsRepository(AppDbContext context) : base(context)
        {
        }
    }
}
