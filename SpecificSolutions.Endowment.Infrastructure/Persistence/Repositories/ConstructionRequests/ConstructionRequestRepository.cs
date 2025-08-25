using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.ConstructionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
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

        public async Task<ConstructionRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.ConstructionRequests.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<ConstructionRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.ConstructionRequests.ToListAsync(cancellationToken);
        }


        public async Task<PagedList<ConstructionRequestDTO>> GetByFilterAsync(FilterConstructionRequestQuery query, CancellationToken cancellationToken)
        {
            var constructionRequests = _context.ConstructionRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                constructionRequests = constructionRequests.Where(cr => cr.ProposedLocation.Contains(query.SearchTerm));
            }

            var constructionRequestDTOs = constructionRequests.Select(cr => new ConstructionRequestDTO
            {
                Id = cr.Id,
                ProposedLocation = cr.ProposedLocation,
                ProposedArea = cr.ProposedArea,
                EstimatedCost = cr.EstimatedCost,
                ContractorName = cr.ContractorName
            });

            return await PagedList<ConstructionRequestDTO>.CreateAsync(constructionRequestDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}