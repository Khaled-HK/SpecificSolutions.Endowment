using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Core.Entities.ConstructionRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.ConstructionRequests
{
    public class ConstructionRequestRepository : Repository<ConstructionRequest>, IConstructionRequestRepository
    {
        private readonly AppDbContext _context;

        public ConstructionRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ConstructionRequest> GetByIdAsync(Guid id)
        {
            return await _context.ConstructionRequests.FindAsync(id);
        }

        public async Task<IEnumerable<ConstructionRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.ConstructionRequests.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(ConstructionRequest constructionRequest, CancellationToken cancellationToken)
        {
            await _context.ConstructionRequests.AddAsync(constructionRequest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(ConstructionRequest constructionRequest)
        {
            _context.ConstructionRequests.Update(constructionRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var constructionRequest = await _context.ConstructionRequests.FindAsync(id);
            if (constructionRequest != null)
            {
                _context.ConstructionRequests.Remove(constructionRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}