using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.FacilityDetails;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.FacilityDetails;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IFacilityDetailRepository : IRepository<FacilityDetail>
    {
        Task<FacilityDetail> GetByIdAsync(Guid id);
        Task<IEnumerable<FacilityDetail>> GetAllAsync();
        Task AddAsync(FacilityDetail FacilityDetail);
        Task UpdateAsync(FacilityDetail FacilityDetail);
        Task DeleteAsync(Guid id);
        Task<PagedList<FacilityDetailDTO>> GetByFilterAsync(FilterFacilityDetailQuery query, CancellationToken cancellationToken);
    }
}