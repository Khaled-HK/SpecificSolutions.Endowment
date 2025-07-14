using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Core.Entities.MaintenanceRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.MaintenanceRequests
{
    public class MaintenanceRequestRepository : Repository<MaintenanceRequest>, IMaintenanceRequestRepository
    {
        private readonly AppDbContext _context;

        public MaintenanceRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<MaintenanceRequest> GetByIdAsync(Guid id)
        {
            return await _context.MaintenanceRequests.FindAsync(id);
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.MaintenanceRequests.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(MaintenanceRequest maintenanceRequest, CancellationToken cancellationToken)
        {
            await _context.MaintenanceRequests.AddAsync(maintenanceRequest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(MaintenanceRequest maintenanceRequest)
        {
            _context.MaintenanceRequests.Update(maintenanceRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var maintenanceRequest = await _context.MaintenanceRequests.FindAsync(id);
            if (maintenanceRequest != null)
            {
                _context.MaintenanceRequests.Remove(maintenanceRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}