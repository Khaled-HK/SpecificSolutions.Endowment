using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetailRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.BuildingDetailRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.BuildingDetailRequests
{
    public class BuildingDetailRequestRepository : Repository<BuildingDetailRequest>, IBuildingDetailRequestRepository
    {
        private readonly AppDbContext _context;

        public BuildingDetailRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BuildingDetailRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.BuildingDetailRequests.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<BuildingDetailRequest>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.BuildingDetailRequests.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(BuildingDetailRequest request, CancellationToken cancellationToken)
        {
            await _context.BuildingDetailRequests.AddAsync(request, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(BuildingDetailRequest request)
        {
            _context.BuildingDetailRequests.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var request = await _context.BuildingDetailRequests.FindAsync(id);
            if (request != null)
            {
                _context.BuildingDetailRequests.Remove(request);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedList<BuildingDetailRequestDTO>> GetByFilterAsync(FilterBuildingDetailRequestQuery query, CancellationToken cancellationToken)
        {
            var requestsQuery = _context.BuildingDetailRequests.AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                requestsQuery = requestsQuery.Where(r =>
                    r.RequestDetails.Contains(query.SearchTerm));
            }

            var dtos = requestsQuery.Select(r => new BuildingDetailRequestDTO
            {
                Id = r.Id,
                RequestDetails = r.RequestDetails,
                RequestDate = r.RequestDate,
                BuildingDetailId = r.BuildingDetailId
            });

            return await PagedList<BuildingDetailRequestDTO>.CreateAsync(dtos, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}