using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Requests
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        private readonly AppDbContext _context;

        public RequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Request> GetByIdAsync(Guid id)
        {
            return await _context.Requests.FindAsync(id);
        }

        public async Task<bool> ReferenceNumberExists(string referenceNumber)
        {
            return await _context.Requests.AnyAsync(r => r.ReferenceNumber == referenceNumber);
        }

        public void Update(Request request)
        {
            _context.Requests.Update(request);
        }

        public async Task AddAsync(Request request)
        {
            await _context.Requests.AddAsync(request);
        }

        public async Task<PagedList<FilterRequestDTO>> GetByFilterAsync(FilterRequestQuery query, CancellationToken cancellationToken)
        {
            var requests = _context.Requests.AsQueryable();

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
            var requestDTOs = requests.Select(r => new FilterRequestDTO
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                CreatedDate = r.CreatedDate,
                ReferenceNumber = r.ReferenceNumber
            }).AsQueryable();

            return await PagedList<FilterRequestDTO>.CreateAsync(requestDTOs, query.PageNumber, (int)query.PageSize, cancellationToken);
        }

        public void Delete(Request request)
        {
            _context.Requests.Remove(request);
        }
    }
}
