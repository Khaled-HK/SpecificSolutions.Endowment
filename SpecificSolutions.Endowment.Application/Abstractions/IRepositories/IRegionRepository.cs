using SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.GetRegions;
using SpecificSolutions.Endowment.Application.Models.DTOs.Regions;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Regions;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IRegionRepository : IRepository<Region>
    {
        Task<Region> GetByIdAsync(Guid id);
        Task<IEnumerable<Region>> GetAllAsync();
        Task AddAsync(Region region);
        Task UpdateAsync(Region region);
        Task DeleteAsync(Guid id);
        Task<PagedList<RegionDTO>> GetByFilterAsync(FilterRegionQuery query, CancellationToken cancellationToken);
        Task<IEnumerable<KeyValuPair>> GetRegionsAsync(GetRegionsQuery query, CancellationToken cancellationToken);
    }
}