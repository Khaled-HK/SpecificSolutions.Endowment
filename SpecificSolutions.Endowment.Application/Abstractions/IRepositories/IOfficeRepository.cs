using SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.GetOffices;
using SpecificSolutions.Endowment.Application.Models.DTOs.Offices;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Offices;
using SpecificSolutions.Endowment.Core.Models;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IOfficeRepository : IRepository<Office>
    {
        Task<Office> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Office office, CancellationToken cancellationToken);
        Task UpdateAsync(Office office);
        Task RemoveAsync(Office office);
        Task<bool> ExistsAsync(Guid id);
        Task<PagedList<FilterOfficeDTO>> GetByFilterAsync(FilterOfficeQuery query, CancellationToken cancellationToken);
        Task<IEnumerable<KeyValuPair>> GetOfficesAsync(GetOfficesQuery query, CancellationToken cancellationToken);
    }
}