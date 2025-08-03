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

        public async Task<Facility> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Facilities.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Facility>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Facilities.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Facility facility, CancellationToken cancellationToken)
        {
            await _context.Facilities.AddAsync(facility, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
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
            var facilitiesQuery = _context.Facilities.AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                facilitiesQuery = facilitiesQuery.Where(f =>
                    f.Name.Contains(query.SearchTerm) ||
                    f.Description.Contains(query.SearchTerm));
            }

            var dtos = facilitiesQuery.Select(f => new FacilityDTO
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                Location = f.Location,
                Capacity = f.Capacity,
                Status = f.Status
            });

            return await PagedList<FacilityDTO>.CreateAsync(dtos, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}