using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Core.Entities.NeedsRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.NeedsRequests
{
    public class NeedsRequestRepository : Repository<NeedsRequest>, INeedsRequestRepository
    {
        private readonly AppDbContext _context;

        public NeedsRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<NeedsRequest> GetByIdAsync(Guid id)
        {
            return await _context.NeedsRequests.FindAsync(id);
        }

        public async Task<IEnumerable<NeedsRequest>> GetAllAsync()
        {
            return await _context.NeedsRequests.ToListAsync();
        }

        public async Task AddAsync(NeedsRequest needsRequest)
        {
            await _context.NeedsRequests.AddAsync(needsRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NeedsRequest needsRequest)
        {
            _context.NeedsRequests.Update(needsRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var needsRequest = await _context.NeedsRequests.FindAsync(id);
            if (needsRequest != null)
            {
                _context.NeedsRequests.Remove(needsRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}