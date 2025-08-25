using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.NeedsRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.NeedsRequests;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface INeedsRequestRepository : IRepository<NeedsRequest>
    {
        Task<NeedsRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        new Task<IEnumerable<NeedsRequest>> GetAllAsync(CancellationToken cancellationToken);
        new Task AddAsync(NeedsRequest request, CancellationToken cancellationToken);
        Task UpdateAsync(NeedsRequest request);
        Task DeleteAsync(Guid id);
        Task<PagedList<NeedsRequestDTO>> GetByFilterAsync(FilterNeedsRequestQuery query, CancellationToken cancellationToken);
    }
}
