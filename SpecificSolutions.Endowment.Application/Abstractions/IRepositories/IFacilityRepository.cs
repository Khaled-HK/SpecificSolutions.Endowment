using SpecificSolutions.Endowment.Application.Handlers.Facilities.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Facilities;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Facilities;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IFacilityRepository : IRepository<Facility>
    {
        Task<Facility> GetByIdAsync(Guid id);
        Task<IEnumerable<Facility>> GetAllAsync();
        Task AddAsync(Facility facility);
        Task UpdateAsync(Facility facility);
        Task DeleteAsync(Guid id);
        Task<PagedList<FacilityDTO>> GetByFilterAsync(FilterFacilityQuery query, CancellationToken cancellationToken);
    }
}