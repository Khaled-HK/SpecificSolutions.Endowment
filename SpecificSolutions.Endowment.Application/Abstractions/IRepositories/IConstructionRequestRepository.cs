using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.ConstructionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.ConstructionRequests;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IConstructionRequestRepository : IRepository<ConstructionRequest>
    {
        Task<ConstructionRequest> GetByIdAsync(Guid id);
        Task<IEnumerable<ConstructionRequest>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(ConstructionRequest constructionRequest, CancellationToken cancellationToken);
        Task UpdateAsync(ConstructionRequest constructionRequest);
        Task DeleteAsync(Guid id);
        Task<PagedList<ConstructionRequestDTO>> GetByFilterAsync(FilterConstructionRequestQuery query, CancellationToken cancellationToken);
    }
}
