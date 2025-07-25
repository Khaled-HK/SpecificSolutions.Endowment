using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetails;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.BuildingDetails;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IBuildingDetailRepository : IRepository<BuildingDetail>
    {
        Task<BuildingDetail> GetByIdAsync(Guid Id, CancellationToken cancellationToken);
        Task<IEnumerable<BuildingDetail>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(BuildingDetail buildingDetail, CancellationToken cancellationToken);
        Task UpdateAsync(BuildingDetail buildingDetail);
        Task DeleteAsync(Guid Id);
        Task<PagedList<BuildingDetailDTO>> GetByFilterAsync(FilterBuildingDetailQuery query, CancellationToken cancellationToken);
    }
}