using SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetailRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.BuildingDetailRequests;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IBuildingDetailRequestRepository : IRepository<BuildingDetailRequest>
    {
        Task<BuildingDetailRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<BuildingDetailRequest>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(BuildingDetailRequest request, CancellationToken cancellationToken);
        Task UpdateAsync(BuildingDetailRequest request);
        Task DeleteAsync(Guid id);
        Task<PagedList<BuildingDetailRequestDTO>> GetByFilterAsync(FilterBuildingDetailRequestQuery query, CancellationToken cancellationToken);
    }
}