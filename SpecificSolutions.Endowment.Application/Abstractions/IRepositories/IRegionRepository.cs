using SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.GetRegions;
using SpecificSolutions.Endowment.Application.Models.DTOs.Regions;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Regions;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IRegionRepository : IRepository<Region>
    {
        Task<Region> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Region>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Region region, CancellationToken cancellationToken);
        Task UpdateAsync(Region region, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<PagedList<FilterRegionDTO>> GetByFilterAsync(FilterRegionQuery query, CancellationToken cancellationToken);
        Task<IEnumerable<KeyValuPair>> GetRegionsAsync(GetRegionsQuery query, CancellationToken cancellationToken);
    }
}