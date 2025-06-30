using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Core.Entities.DemolitionRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.DemolitionRequests
{
    public class DemolitionRequestRepository : Repository<DemolitionRequest>, IDemolitionRequestRepository
    {
        private readonly AppDbContext _context;

        public DemolitionRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<DemolitionRequest> GetByIdAsync(Guid id)
        {
            return await _context.DemolitionRequests.FindAsync(id);
        }

        public async Task<IEnumerable<DemolitionRequest>> GetAllAsync()
        {
            return await _context.DemolitionRequests.ToListAsync();
        }

        public async Task AddAsync(DemolitionRequest DemolitionRequest)
        {
            await _context.DemolitionRequests.AddAsync(DemolitionRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DemolitionRequest DemolitionRequest)
        {
            _context.DemolitionRequests.Update(DemolitionRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var DemolitionRequest = await _context.DemolitionRequests.FindAsync(id);
            if (DemolitionRequest != null)
            {
                _context.DemolitionRequests.Remove(DemolitionRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}