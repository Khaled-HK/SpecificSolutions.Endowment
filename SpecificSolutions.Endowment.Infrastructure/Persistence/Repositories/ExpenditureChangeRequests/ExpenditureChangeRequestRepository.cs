using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.ExpenditureChangeRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.ExpenditureChangeRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.EndowmentExpenditureChangeRequests
{
    public class ExpenditureChangeRequestRepository : Repository<ExpenditureChangeRequest>, IExpenditureChangeRequestRepository
    {
        private readonly AppDbContext _context;

        public ExpenditureChangeRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ExpenditureChangeRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.ExpenditureChangeRequests.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<ExpenditureChangeRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.ExpenditureChangeRequests.ToListAsync(cancellationToken);
        }


        public async Task<PagedList<ExpenditureChangeRequestDTO>> GetByFilterAsync(FilterExpenditureChangeRequestQuery query, CancellationToken cancellationToken)
        {
            var expenditureChangeRequests = _context.ExpenditureChangeRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                expenditureChangeRequests = expenditureChangeRequests.Where(ecr =>
                    ecr.Reason.Contains(query.SearchTerm) ||
                    (ecr.Request != null && (
                        ecr.Request.Title.Contains(query.SearchTerm) ||
                        ecr.Request.Description.Contains(query.SearchTerm) ||
                        ecr.Request.ReferenceNumber.Contains(query.SearchTerm)
                    )));
            }

            var expenditureChangeRequestDTOs = expenditureChangeRequests.Select(ecr => new ExpenditureChangeRequestDTO
            {
                Id = ecr.Id,
                // Base FilterRequestDTO properties (from Request)
                Title = ecr.Request != null ? ecr.Request.Title : null,
                Description = ecr.Request != null ? ecr.Request.Description : null,
                CreatedDate = ecr.Request != null ? ecr.Request.CreatedDate : default,
                ReferenceNumber = ecr.Request != null ? ecr.Request.ReferenceNumber : null,
                SubmissionDate = ecr.Request != null ? ecr.Request.CreatedDate : default,
                Priority = "Medium", // Default value
                Location = "N/A", // Default value
                RequestStatus = "Pending", // Default value
                Attachments = new List<string>(), // Default empty list
                // Specific ExpenditureChange fields
                CurrentExpenditure = ecr.CurrentExpenditureBranch != null ? ecr.CurrentExpenditureBranch.Name : string.Empty,
                NewExpenditure = ecr.NewExpenditureBranch != null ? ecr.NewExpenditureBranch.Name : string.Empty,
                Reason = ecr.Reason,
                ApprovalRequired = true // Default value
            });

            return await PagedList<ExpenditureChangeRequestDTO>.CreateAsync(expenditureChangeRequestDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}