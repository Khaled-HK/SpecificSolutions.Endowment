using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.GetOffices;
using SpecificSolutions.Endowment.Application.Models.DTOs.Offices;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Offices;
using SpecificSolutions.Endowment.Core.Models;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Offices
{
    public class OfficeRepository : Repository<Office>, IOfficeRepository
    {
        private readonly AppDbContext _context;

        public OfficeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Office> GetByIdAsync(Guid id)
        {
            return await _context.Offices.FindAsync(id);
        }

        public async Task AddAsync(Office office)
        {
            await _context.Offices.AddAsync(office);
        }

        public async Task UpdateAsync(Office office)
        {
            _context.Offices.Update(office);
        }

        public async Task RemoveAsync(Office office)
        {
            _context.Offices.Remove(office);
        }

        //todo //Guid
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Offices.AnyAsync(o => o.Id == id);
        }

        public async Task<PagedList<FilterOfficeDTO>> GetByFilterAsync(FilterOfficeQuery query, CancellationToken cancellationToken)
        {
            var officesQuery = _context.Offices.AsQueryable();

            // Apply filtering based on the query parameters
            if (!string.IsNullOrEmpty(query.Name))
            {
                officesQuery = officesQuery.Where(o => o.Name.Contains(query.Name));
            }

            // Select the relevant fields to return as DTOs
            var officeDTOs = officesQuery.Select(o => new FilterOfficeDTO
            {
                Id = o.Id,
                Name = o.Name,
                PhoneNumber = o.PhoneNumber
            });

            // Return paged results
            return await PagedList<FilterOfficeDTO>.CreateAsync(officeDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }

        public async Task<IEnumerable<KeyValuPair>> GetOfficesAsync(GetOfficesQuery query, CancellationToken cancellationToken)
        {
            var officesQuery = _context.Offices.AsQueryable();
            // Apply filtering based on the query parameters
            if (!string.IsNullOrEmpty(query.Name))
            {
                officesQuery = officesQuery.Where(o => o.Name.Contains(query.Name));
            }
            // Select the relevant fields to return as DTOs
            var officeDTOs = officesQuery.Select(o => new KeyValuPair
            {
                Key = o.Id,
                Value = o.Name,
            });
            // Return paged results
            return await officeDTOs.ToListAsync();
        }
    }
}