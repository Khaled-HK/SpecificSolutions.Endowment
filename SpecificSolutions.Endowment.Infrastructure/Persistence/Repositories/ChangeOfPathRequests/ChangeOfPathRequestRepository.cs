using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Core.Entities.ChangeOfPathRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.ChangeOfPathRequests
{
    public class ChangeOfPathRequestRepository : Repository<ChangeOfPathRequest>, IChangeOfPathRequestRepository
    {
        private readonly AppDbContext _context;

        public ChangeOfPathRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ChangeOfPathRequest> GetByIdAsync(Guid id)
        {
            return await _context.ChangeOfPathRequests.FindAsync(id);
        }

        public async Task<IEnumerable<ChangeOfPathRequest>> GetAllAsync()
        {
            return await _context.ChangeOfPathRequests.ToListAsync();
        }

        public async Task AddAsync(ChangeOfPathRequest changeOfPathRequest)
        {
            await _context.ChangeOfPathRequests.AddAsync(changeOfPathRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChangeOfPathRequest changeOfPathRequest)
        {
            _context.ChangeOfPathRequests.Update(changeOfPathRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var changeOfPathRequest = await _context.ChangeOfPathRequests.FindAsync(id);
            if (changeOfPathRequest != null)
            {
                _context.ChangeOfPathRequests.Remove(changeOfPathRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}