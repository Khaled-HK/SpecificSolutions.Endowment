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

        public async Task<FacilityDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.FacilityDetails.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<FacilityDetail>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.FacilityDetails.ToListAsync(cancellationToken);
        }

        //public async Task AddAsync(FacilityDetail FacilityDetail, CancellationToken cancellationToken)
        //{
        //    await _context.FacilityDetails.AddAsync(FacilityDetail, cancellationToken);
        //    await _context.SaveChangesAsync(cancellationToken);
        //}

        //public async Task UpdateAsync(FacilityDetail FacilityDetail)
        //{
        //    _context.FacilityDetails.Update(FacilityDetail);
        //    await _context.SaveChangesAsync();
        //}

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
            var facilityDetailsQuery = _context.FacilityDetails.AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchTerm))
                facilityDetailsQuery = facilityDetailsQuery.Where(fd => fd.Product.Name.Contains(query.SearchTerm));

            var dtos = facilityDetailsQuery.Select(fd => new FacilityDetailDTO
            {
                Id = fd.Id,
                Quantity = fd.Quantity,
                ProductId = fd.ProductId,
                ProductName = fd.Product.Name
            });

            return await PagedList<FacilityDetailDTO>.CreateAsync(dtos, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}