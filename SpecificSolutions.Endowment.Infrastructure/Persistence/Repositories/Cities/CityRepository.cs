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

        public async Task<City> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Cities.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<City>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Cities.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(City city, CancellationToken cancellationToken)
        {
            await _context.Cities.AddAsync(city, cancellationToken);
            // Remove SaveChangesAsync here as it should be handled by UnitOfWork
        }

        public async Task UpdateAsync(City city)
        {
            _context.Cities.Update(city);
            // Remove SaveChangesAsync here as it should be handled by UnitOfWork
        }

        public async Task DeleteAsync(Guid id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city != null)
            {
                _context.Cities.Remove(city);
                // Remove SaveChangesAsync here as it should be handled by UnitOfWork
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

        public async Task<bool> GetRelatedDataAsync(Guid cityId)
        {
            // Check for Regions related to this city
            var hasRelatedData = await _context.Regions
                .AnyAsync(r => r.CityId == cityId);

            return hasRelatedData;
        }
    }
}