using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.GetOffices;
using SpecificSolutions.Endowment.Application.Models.DTOs.Offices;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Offices;
using Microsoft.EntityFrameworkCore;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Offices
{
    public class OfficeRepository : Repository<Office>, IOfficeRepository
    {
        private readonly AppDbContext _context;

        public OfficeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Office> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Offices.FindAsync(id, cancellationToken);
        }

        public async Task AddAsync(Office office, CancellationToken cancellationToken)
        {
            await _context.Offices.AddAsync(office, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
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
            var officesQuery = _context.Offices
                .Include(o => o.Region)
                .AsQueryable();

            // Apply filtering based on the query parameters
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                officesQuery = officesQuery.Where(o => 
                    o.Name.Contains(query.SearchTerm) ||
                    o.PhoneNumber.Contains(query.SearchTerm) ||
                    (o.Region != null && o.Region.Name.Contains(query.SearchTerm)));
            }

            if (!string.IsNullOrEmpty(query.Name))
            {
                officesQuery = officesQuery.Where(o => o.Name.Contains(query.Name));
            }

            // Select the relevant fields to return as DTOs
            var officeDTOs = officesQuery.Select(o => new FilterOfficeDTO
            {
                Id = o.Id,
                Name = o.Name,
                PhoneNumber = o.PhoneNumber,
                RegionName = o.Region != null ? o.Region.Name : string.Empty
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