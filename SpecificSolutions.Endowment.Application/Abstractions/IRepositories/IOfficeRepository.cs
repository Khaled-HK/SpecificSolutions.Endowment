using SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.GetOffices;
using SpecificSolutions.Endowment.Application.Models.DTOs.Offices;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Offices;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IOfficeRepository : IRepository<Office>
    {
        Task<Office> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        new Task<IEnumerable<Office>> GetAllAsync(CancellationToken cancellationToken);
        new Task AddAsync(Office office, CancellationToken cancellationToken);
        Task UpdateAsync(Office office);
        new Task RemoveAsync(Office office);
        Task DeleteAsync(Guid id);
        Task<PagedList<FilterOfficeDTO>> GetByFilterAsync(FilterOfficeQuery query, CancellationToken cancellationToken);
        Task<IEnumerable<KeyValuePair<string, string>>> GetOfficesAsync(GetOfficesQuery query, CancellationToken cancellationToken);
        Task<bool> GetRelatedDataAsync(Guid id, CancellationToken cancellationToken);
    }
}