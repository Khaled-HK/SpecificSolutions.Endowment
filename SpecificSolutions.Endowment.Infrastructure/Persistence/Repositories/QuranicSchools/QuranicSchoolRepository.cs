using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.QuranicSchools;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.QuranicSchools;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.QuranicSchools
{
    public class QuranicSchoolRepository : Repository<QuranicSchool>, IQuranicSchoolRepository
    {
        private readonly AppDbContext _context;

        public QuranicSchoolRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<QuranicSchool> GetByIdAsync(Guid id)
        {
            return await _context.QuranicSchools.FindAsync(id);
        }

        public async Task<IEnumerable<QuranicSchool>> GetAllAsync()
        {
            return await _context.QuranicSchools.ToListAsync();
        }

        public async Task AddAsync(QuranicSchool quranicSchool)
        {
            await _context.QuranicSchools.AddAsync(quranicSchool);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(QuranicSchool quranicSchool)
        {
            _context.QuranicSchools.Update(quranicSchool);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var quranicSchool = await _context.QuranicSchools.FindAsync(id);
            if (quranicSchool != null)
            {
                _context.QuranicSchools.Remove(quranicSchool);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PagedList<QuranicSchoolDTO>> GetByFilterAsync(FilterQuranicSchoolQuery query, CancellationToken cancellationToken)
        {
            var quranicSchoolQuery = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                quranicSchoolQuery = quranicSchoolQuery.Where(p =>
                    p.Name.Contains(query.SearchTerm) ||
                    p.Description.Contains(query.SearchTerm));
            }

            var dtos = quranicSchoolQuery.Select(p => new QuranicSchoolDTO
            {
                Id = p.Id,

            });

            return await PagedList<QuranicSchoolDTO>.CreateAsync(dtos, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}