using SpecificSolutions.Endowment.Application.Handlers.Facilities.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Facilities;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Facilities;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IFacilityRepository : IRepository<Facility>
    {
        Task<Facility> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Facility>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Facility facility, CancellationToken cancellationToken);
        Task UpdateAsync(Facility facility);
        Task DeleteAsync(Guid id);
        Task<PagedList<FacilityDTO>> GetByFilterAsync(FilterFacilityQuery query, CancellationToken cancellationToken);
    }
}