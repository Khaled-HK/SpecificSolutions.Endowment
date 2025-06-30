using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Core.Entities.NameChangeRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.NameChangeRequests
{
    public class NameChangeRequestRepository : Repository<NameChangeRequest>, INameChangeRequestRepository
    {
        private readonly AppDbContext _context;

        public NameChangeRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<NameChangeRequest> GetByIdAsync(Guid id)
        {
            return await _context.NameChangeRequests.FindAsync(id);
        }

        public async Task<IEnumerable<NameChangeRequest>> GetAllAsync()
        {
            return await _context.NameChangeRequests.ToListAsync();
        }

        public async Task AddAsync(NameChangeRequest nameChangeRequest)
        {
            await _context.NameChangeRequests.AddAsync(nameChangeRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NameChangeRequest nameChangeRequest)
        {
            _context.NameChangeRequests.Update(nameChangeRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var nameChangeRequest = await _context.NameChangeRequests.FindAsync(id);
            if (nameChangeRequest != null)
            {
                _context.NameChangeRequests.Remove(nameChangeRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}