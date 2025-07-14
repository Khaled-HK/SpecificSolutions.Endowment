using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.GetDecisions;
using SpecificSolutions.Endowment.Application.Models.DTOs.Decisions;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Decisions;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Decisions
{
    public class DecisionRepository : Repository<Decision>, IDecisionRepository
    {
        private readonly AppDbContext _context;

        public DecisionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Decision> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Decisions.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Decision>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Decisions.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Decision decision, CancellationToken cancellationToken)
        {
            await _context.Decisions.AddAsync(decision, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Update(Decision decision)
        {
            _context.Decisions.Update(decision);
        }

        public void Delete(Decision decision)
        {
            _context.Decisions.Remove(decision);
        }

        public async Task<PagedList<FilterDecisionDTO>> GetByFilterAsync(FilterDecisionQuery query, CancellationToken cancellationToken)
        {
            var requests = _context.Decisions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                requests = requests.Where(r => r.Title.Contains(query.Title));
            }

            if (!string.IsNullOrWhiteSpace(query.ReferenceNumber))
            {
                requests = requests.Where(r => r.ReferenceNumber.Contains(query.ReferenceNumber));
            }

            if (query.CreatedDate.HasValue)
            {
                requests = requests.Where(r => r.CreatedDate.Date == query.CreatedDate.Value.Date);
            }
            var requestDTOs = requests.Select(r => new FilterDecisionDTO
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                CreatedDate = r.CreatedDate,
                ReferenceNumber = r.ReferenceNumber
            }).AsQueryable();

            return await PagedList<FilterDecisionDTO>.CreateAsync(requestDTOs, query.PageNumber, (int)query.PageSize, cancellationToken);
        }

        public async Task<IEnumerable<KeyValuPair>> GetDecisionsAsync(GetDecisionsQuery query, CancellationToken cancellationToken)
        {
            var officesQuery = _context.Decisions.AsQueryable();
            // Apply filtering based on the query parameters
            if (!string.IsNullOrEmpty(query.Name))
            {
                officesQuery = officesQuery.Where(o => o.ReferenceNumber.Contains(query.Name));
            }
            // Select the relevant fields to return as DTOs
            var officeDTOs = officesQuery.Select(o => new KeyValuPair
            {
                Key = o.Id,
                Value = o.ReferenceNumber,
            });
            // Return paged results
            return await officeDTOs.ToListAsync();
        }
    }
}