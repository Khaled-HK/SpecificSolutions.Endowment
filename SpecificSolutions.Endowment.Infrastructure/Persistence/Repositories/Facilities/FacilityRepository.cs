using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Facilities;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Facilities;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Facilities
{
    public class FacilityRepository : Repository<Facility>, IFacilityRepository
    {
        private readonly AppDbContext _context;

        public FacilityRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Facility> GetByIdAsync(Guid id)
        {
            return await _context.Facilities.FindAsync(id);
        }

        public async Task<IEnumerable<Facility>> GetAllAsync()
        {
            return await _context.Facilities.ToListAsync();
        }

        public async Task AddAsync(Facility facility)
        {
            await _context.Facilities.AddAsync(facility);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Facility facility)
        {
            _context.Facilities.Update(facility);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility != null)
            {
                _context.Facilities.Remove(facility);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedList<FacilityDTO>> GetByFilterAsync(FilterFacilityQuery query, CancellationToken cancellationToken)
        {
            var citiesQuery = _context.Cities.AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                citiesQuery = citiesQuery.Where(c =>
                    c.Name.Contains(query.SearchTerm) ||
                    c.Country.Contains(query.SearchTerm));
            }

            var dtos = citiesQuery.Select(c => new FacilityDTO
            {
                Id = c.Id,
                Name = c.Name,
            });

            return await PagedList<FacilityDTO>.CreateAsync(dtos, query.PageNumber, query.PageSize, cancellationToken);
        }

    }
}