using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.FacilityDetails;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.FacilityDetails;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.FacilityDetails
{
    public class FacilityDetailRepository : Repository<FacilityDetail>, IFacilityDetailRepository
    {
        private readonly AppDbContext _context;

        public FacilityDetailRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<FacilityDetail> GetByIdAsync(Guid id)
        {
            return await _context.FacilityDetails.FindAsync(id);
        }

        public async Task<IEnumerable<FacilityDetail>> GetAllAsync()
        {
            return await _context.FacilityDetails.ToListAsync();
        }

        public async Task AddAsync(FacilityDetail FacilityDetail)
        {
            await _context.FacilityDetails.AddAsync(FacilityDetail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FacilityDetail FacilityDetail)
        {
            _context.FacilityDetails.Update(FacilityDetail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var FacilityDetail = await _context.FacilityDetails.FindAsync(id);
            if (FacilityDetail != null)
            {
                _context.FacilityDetails.Remove(FacilityDetail);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedList<FacilityDetailDTO>> GetByFilterAsync(FilterFacilityDetailQuery query, CancellationToken cancellationToken)
        {
            var citiesQuery = _context.Cities.AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                citiesQuery = citiesQuery.Where(c =>
                    c.Name.Contains(query.SearchTerm) ||
                    c.Country.Contains(query.SearchTerm));
            }

            var dtos = citiesQuery.Select(c => new FacilityDetailDTO
            {
                Id = c.Id,
                Name = c.Name,
            });

            return await PagedList<FacilityDetailDTO>.CreateAsync(dtos, query.PageNumber, query.PageSize, cancellationToken);
        }

    }
}