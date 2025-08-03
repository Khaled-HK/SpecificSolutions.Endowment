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

        public async Task<NeedsRequest> GetByIdAsync(Guid id)
        {
            return await _context.NeedsRequests.FindAsync(id);
        }

        public async Task<IEnumerable<NeedsRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.NeedsRequests.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(NeedsRequest needsRequest, CancellationToken cancellationToken)
        {
            await _context.NeedsRequests.AddAsync(needsRequest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
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

        public async Task<PagedList<NeedsRequestDTO>> GetByFilterAsync(FilterNeedsRequestQuery query, CancellationToken cancellationToken)
        {
            var needsRequests = _context.NeedsRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                needsRequests = needsRequests.Where(nr => 
                    nr.Description.Contains(query.SearchTerm) || 
                    nr.Priority.ToString().Contains(query.SearchTerm));
            }

            var needsRequestDTOs = needsRequests.Select(nr => new NeedsRequestDTO
            {
                Id = nr.Id,
                Description = nr.Description,
                Priority = nr.Priority,
                EstimatedCost = nr.EstimatedCost,
                RequestDate = nr.RequestDate
            });

            return await PagedList<NeedsRequestDTO>.CreateAsync(needsRequestDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}