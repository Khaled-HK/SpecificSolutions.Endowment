using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.ConstructionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.ConstructionRequests;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IConstructionRequestRepository : IRepository<ConstructionRequest>
    {
        Task<ConstructionRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        new Task<IEnumerable<ConstructionRequest>> GetAllAsync(CancellationToken cancellationToken);
        new Task AddAsync(ConstructionRequest request, CancellationToken cancellationToken);
        Task UpdateAsync(ConstructionRequest request);
        Task DeleteAsync(Guid id);
        Task<PagedList<ConstructionRequestDTO>> GetByFilterAsync(FilterConstructionRequestQuery query, CancellationToken cancellationToken);
    }
}
