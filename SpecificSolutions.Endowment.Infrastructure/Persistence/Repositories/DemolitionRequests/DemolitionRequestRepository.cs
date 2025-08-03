using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.DemolitionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
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

        public async Task<IEnumerable<DemolitionRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.DemolitionRequests.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(DemolitionRequest DemolitionRequest, CancellationToken cancellationToken)
        {
            await _context.DemolitionRequests.AddAsync(DemolitionRequest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
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

        public async Task<PagedList<FilterDemolitionRequestDTO>> GetByFilterAsync(FilterDemolitionRequestQuery query, CancellationToken cancellationToken)
        {
            var demolitionRequests = _context.DemolitionRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                demolitionRequests = demolitionRequests.Where(dr => dr.ContractorName.Contains(query.SearchTerm));
            }

            var demolitionRequestDTOs = demolitionRequests.Select(dr => new FilterDemolitionRequestDTO
            {
                Id = dr.Id,
                Reason = dr.Reason
            });

            return await PagedList<FilterDemolitionRequestDTO>.CreateAsync(demolitionRequestDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}