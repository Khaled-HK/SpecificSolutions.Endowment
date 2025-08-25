using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.DemolitionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.DemolitionRequests;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IDemolitionRequestRepository : IRepository<DemolitionRequest>
    {
        Task<DemolitionRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        new Task<IEnumerable<DemolitionRequest>> GetAllAsync(CancellationToken cancellationToken);
        new Task AddAsync(DemolitionRequest request, CancellationToken cancellationToken);
        Task UpdateAsync(DemolitionRequest request);
        Task DeleteAsync(Guid id);
        Task<PagedList<FilterDemolitionRequestDTO>> GetByFilterAsync(FilterDemolitionRequestQuery query, CancellationToken cancellationToken);
    }
}
