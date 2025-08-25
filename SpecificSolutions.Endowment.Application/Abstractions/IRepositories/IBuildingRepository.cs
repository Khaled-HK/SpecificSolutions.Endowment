using SpecificSolutions.Endowment.Application.Handlers.Buildings.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Buildings;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Buildings;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IBuildingRepository : IRepository<Building>
    {
        Task<Building> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        new Task<IEnumerable<Building>> GetAllAsync(CancellationToken cancellationToken);
        new Task AddAsync(Building building, CancellationToken cancellationToken);
        Task UpdateAsync(Building building);
        Task DeleteAsync(Guid id);
        Task<PagedList<BuildingDTO>> GetByFilterAsync(FilterBuildingQuery query, CancellationToken cancellationToken);
    }
}
