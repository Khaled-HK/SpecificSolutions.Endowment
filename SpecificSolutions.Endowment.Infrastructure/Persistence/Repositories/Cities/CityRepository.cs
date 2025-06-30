using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Cities;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Cities;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Cities
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly AppDbContext _context;

        public CityRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<City> GetByIdAsync(Guid id)
        {
            return await _context.Cities.FindAsync(id);
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task AddAsync(City city)
        {
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(City city)
        {
            _context.Cities.Update(city);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city != null)
            {
                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedList<CityDTO>> GetByFilterAsync(FilterCityQuery query, CancellationToken cancellationToken)
        {
            var citiesQuery = _context.Cities.AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                citiesQuery = citiesQuery.Where(c =>
                    c.Name.Contains(query.SearchTerm) ||
                    c.Country.Contains(query.SearchTerm));
            }

            var dtos = citiesQuery.Select(c => new CityDTO
            {
                Id = c.Id,
                Name = c.Name,
                Country = c.Country
            });

            return await PagedList<CityDTO>.CreateAsync(dtos, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}