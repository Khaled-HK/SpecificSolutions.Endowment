using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.NameChangeRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.NameChangeRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.NameChangeRequests
{
    public class NameChangeRequestRepository : Repository<NameChangeRequest>, INameChangeRequestRepository
    {
        private readonly AppDbContext _context;

        public NameChangeRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<NameChangeRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.NameChangeRequests.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<NameChangeRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.NameChangeRequests.ToListAsync(cancellationToken);
        }

        public async Task<PagedList<NameChangeRequestDTO>> GetByFilterAsync(FilterNameChangeRequestQuery query, CancellationToken cancellationToken)
        {
            var nameChangeRequests = _context.NameChangeRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                nameChangeRequests = nameChangeRequests.Where(ncr =>
                    ncr.CurrentName.Contains(query.SearchTerm) ||
                    ncr.NewName.Contains(query.SearchTerm) ||
                    ncr.Reason.Contains(query.SearchTerm) ||
                    (ncr.Request != null && (
                        ncr.Request.Title.Contains(query.SearchTerm) ||
                        ncr.Request.Description.Contains(query.SearchTerm) ||
                        ncr.Request.ReferenceNumber.Contains(query.SearchTerm)
                    )));
            }

            var nameChangeRequestDTOs = nameChangeRequests.Select(ncr => new NameChangeRequestDTO
            {
                Id = ncr.Id,
                // Base FilterRequestDTO properties (from Request)
                Title = ncr.Request != null ? ncr.Request.Title : null,
                Description = ncr.Request != null ? ncr.Request.Description : null,
                CreatedDate = ncr.Request != null ? ncr.Request.CreatedDate : default,
                ReferenceNumber = ncr.Request != null ? ncr.Request.ReferenceNumber : null,
                SubmissionDate = ncr.Request != null ? ncr.Request.CreatedDate : default,
                Priority = "Medium", // Default value
                Location = "N/A", // Default value
                RequestStatus = "Pending", // Default value
                Attachments = new List<string>(), // Default empty list
                // Specific NameChange fields
                CurrentName = ncr.CurrentName,
                NewName = ncr.NewName,
                Reason = ncr.Reason,
                BuildingType = "Residential" // Default value
            });

            return await PagedList<NameChangeRequestDTO>.CreateAsync(nameChangeRequestDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}