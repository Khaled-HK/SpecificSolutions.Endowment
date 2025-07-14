using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Core.Entities.EndowmentExpenditureChangeRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.EndowmentExpenditureChangeRequests
{
    public class ExpenditureChangeRequestRepository : Repository<ExpenditureChangeRequest>, IExpenditureChangeRequestRepository
    {
        private readonly AppDbContext _context;

        public ExpenditureChangeRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ExpenditureChangeRequest> GetByIdAsync(Guid id)
        {
            return await _context.ExpenditureChangeRequests.FindAsync(id);
        }

        public async Task<IEnumerable<ExpenditureChangeRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.ExpenditureChangeRequests.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(ExpenditureChangeRequest endowmentExpenditureChangeRequest, CancellationToken cancellationToken)
        {
            await _context.ExpenditureChangeRequests.AddAsync(endowmentExpenditureChangeRequest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(ExpenditureChangeRequest endowmentExpenditureChangeRequest)
        {
            _context.ExpenditureChangeRequests.Update(endowmentExpenditureChangeRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var endowmentExpenditureChangeRequest = await _context.ExpenditureChangeRequests.FindAsync(id);
            if (endowmentExpenditureChangeRequest != null)
            {
                _context.ExpenditureChangeRequests.Remove(endowmentExpenditureChangeRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}