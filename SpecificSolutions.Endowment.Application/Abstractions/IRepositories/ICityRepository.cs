using SpecificSolutions.Endowment.Application.Handlers.Cities.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Cities;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Cities;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface ICityRepository : IRepository<City>
    {
        Task<City> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        new Task<IEnumerable<City>> GetAllAsync(CancellationToken cancellationToken);
        new Task AddAsync(City city, CancellationToken cancellationToken);
        Task UpdateAsync(City city);
        Task DeleteAsync(Guid id);
        Task<PagedList<CityDTO>> GetByFilterAsync(FilterCityQuery query, CancellationToken cancellationToken);
        Task<bool> GetRelatedDataAsync(Guid id, CancellationToken cancellationToken);
    }
}