using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.ChangeOfPathRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.ChangeOfPathRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.ChangeOfPathRequests
{
    public class ChangeOfPathRequestRepository : Repository<ChangeOfPathRequest>, IChangeOfPathRequestRepository
    {
        private readonly AppDbContext _context;

        public ChangeOfPathRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ChangeOfPathRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.ChangeOfPathRequests.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<ChangeOfPathRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.ChangeOfPathRequests.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(ChangeOfPathRequest changeOfPathRequest, CancellationToken cancellationToken)
        {
            await _context.ChangeOfPathRequests.AddAsync(changeOfPathRequest, cancellationToken);
        }

        public async Task UpdateAsync(ChangeOfPathRequest changeOfPathRequest)
        {
            _context.ChangeOfPathRequests.Update(changeOfPathRequest);
        }

        public async Task DeleteAsync(Guid id)
        {
            var changeOfPathRequest = await _context.ChangeOfPathRequests.FindAsync(id);
            if (changeOfPathRequest != null)
            {
                _context.ChangeOfPathRequests.Remove(changeOfPathRequest);
            }
        }

        public async Task<PagedList<ChangeOfPathRequestDTO>> GetByFilterAsync(FilterChangeOfPathRequestQuery query, CancellationToken cancellationToken)
        {
            var changeOfPathRequests = _context.ChangeOfPathRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                changeOfPathRequests = changeOfPathRequests.Where(cpr => 
                    cpr.CurrentType.Contains(query.SearchTerm) || 
                    cpr.NewType.Contains(query.SearchTerm) || 
                    cpr.Reason.Contains(query.SearchTerm));
            }

            var changeOfPathRequestDTOs = changeOfPathRequests.Select(cpr => new ChangeOfPathRequestDTO
            {
                Id = cpr.Id,
                CurrentType = cpr.CurrentType,
                NewType = cpr.NewType,
                Reason = cpr.Reason
            });

            return await PagedList<ChangeOfPathRequestDTO>.CreateAsync(changeOfPathRequestDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}