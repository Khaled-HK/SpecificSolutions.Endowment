using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.NeedsRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
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

        public async Task<NeedsRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.NeedsRequests.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<NeedsRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.NeedsRequests.ToListAsync(cancellationToken);
        }

        public async Task<PagedList<NeedsRequestDTO>> GetByFilterAsync(FilterNeedsRequestQuery query, CancellationToken cancellationToken)
        {
            var needsRequests = _context.NeedsRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                needsRequests = needsRequests.Where(nr =>
                    nr.NeedsType.Contains(query.SearchTerm) ||
                    nr.Location.Contains(query.SearchTerm) ||
                    nr.Provider.Contains(query.SearchTerm));
            }

            var needsRequestDTOs = needsRequests.Select(nr => new NeedsRequestDTO
            {
                Id = nr.Id,
                NeedsType = nr.NeedsType,
                Location = nr.Location,
                EstimatedCost = nr.EstimatedCost,
                Provider = nr.Provider,
                RequestId = nr.RequestId
            });

            return await PagedList<NeedsRequestDTO>.CreateAsync(needsRequestDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}