using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.GetRegions;
using SpecificSolutions.Endowment.Application.Models.DTOs.Regions;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Regions;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Regions
{
    public class RegionRepository : Repository<Region>, IRegionRepository
    {
        private readonly AppDbContext _context;

        public RegionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Region> GetByIdAsync(Guid id)
        {
            return await _context.Regions.FindAsync(id);
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _context.Regions.ToListAsync();
        }

        public async Task AddAsync(Region region)
        {
            await _context.Regions.AddAsync(region);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Region region)
        {
            _context.Regions.Update(region);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region != null)
            {
                _context.Regions.Remove(region);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedList<RegionDTO>> GetByFilterAsync(FilterRegionQuery query, CancellationToken cancellationToken)
        {
            var regionsQuery = _context.Regions.AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                regionsQuery = regionsQuery.Where(r =>
                    r.Name.Contains(query.SearchTerm) ||
                    r.Country.Contains(query.SearchTerm));
            }

            var dtos = regionsQuery.Select(r => new RegionDTO
            {
                Id = r.Id,
                Name = r.Name,
                Country = r.Country
            });

            return await PagedList<RegionDTO>.CreateAsync(dtos, query.PageNumber, query.PageSize, cancellationToken);
        }

        public async Task<IEnumerable<KeyValuPair>> GetRegionsAsync(GetRegionsQuery query, CancellationToken cancellationToken)
        {
            var regionsQuery = _context.Regions.AsQueryable();
            // Apply filtering based on the query parameters
            if (!string.IsNullOrEmpty(query.Name))
            {
                regionsQuery = regionsQuery.Where(o => o.Name.Contains(query.Name));
            }
            // Select the relevant fields to return as DTOs
            var officeDTOs = regionsQuery.Select(o => new KeyValuPair
            {
                Key = o.Id,
                Value = o.Name,
            });
            // Return paged results
            return await officeDTOs.ToListAsync();
        }
    }
}