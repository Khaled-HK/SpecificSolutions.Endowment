using SpecificSolutions.Endowment.Application.Handlers.Cities.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Cities;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Cities;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface ICityRepository : IRepository<City>
    {
        Task<City> GetByIdAsync(Guid Id);
        Task<IEnumerable<City>> GetAllAsync();
        Task AddAsync(City city);
        Task UpdateAsync(City city);
        Task DeleteAsync(Guid Id);
        Task<PagedList<CityDTO>> GetByFilterAsync(FilterCityQuery query, CancellationToken cancellationToken);
    }
}