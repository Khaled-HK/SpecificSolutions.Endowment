using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.MaintenanceRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.MaintenanceRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.MaintenanceRequests
{
    public class MaintenanceRequestRepository : Repository<MaintenanceRequest>, IMaintenanceRequestRepository
    {
        private readonly AppDbContext _context;

        public MaintenanceRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<MaintenanceRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.MaintenanceRequests.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.MaintenanceRequests.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(MaintenanceRequest maintenanceRequest, CancellationToken cancellationToken)
        {
            await _context.MaintenanceRequests.AddAsync(maintenanceRequest, cancellationToken);
        }

        public async Task UpdateAsync(MaintenanceRequest maintenanceRequest)
        {
            _context.MaintenanceRequests.Update(maintenanceRequest);
        }

        public async Task DeleteAsync(Guid id)
        {
            var maintenanceRequest = await _context.MaintenanceRequests.FindAsync(id);
            if (maintenanceRequest != null)
            {
                _context.MaintenanceRequests.Remove(maintenanceRequest);
            }
        }

        public async Task<PagedList<MaintenanceRequestDTO>> GetByFilterAsync(FilterMaintenanceRequestQuery query, CancellationToken cancellationToken)
        {
            var maintenanceRequests = _context.MaintenanceRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                maintenanceRequests = maintenanceRequests.Where(mr => 
                    mr.Location.Contains(query.SearchTerm) ||
                    (mr.Request != null && (
                        mr.Request.Title.Contains(query.SearchTerm) ||
                        mr.Request.Description.Contains(query.SearchTerm) ||
                        mr.Request.ReferenceNumber.Contains(query.SearchTerm)
                    )));
            }

            var maintenanceRequestDTOs = maintenanceRequests.Select(mr => new MaintenanceRequestDTO
            {
                Id = mr.Id,
                // Base FilterRequestDTO properties (from Request)
                Title = mr.Request != null ? mr.Request.Title : null,
                Description = mr.Request != null ? mr.Request.Description : null,
                CreatedDate = mr.Request != null ? mr.Request.CreatedDate : default,
                ReferenceNumber = mr.Request != null ? mr.Request.ReferenceNumber : null,
                SubmissionDate = mr.Request != null ? mr.Request.CreatedDate : default,
                Priority = "Medium", // Default value
                Location = mr.Location,
                RequestStatus = "Pending", // Default value
                Attachments = new List<string>(), // Default empty list
                // Specific Maintenance fields
                MaintenanceType = mr.MaintenanceType,
                EstimatedCost = mr.EstimatedCost,
                ExpectedStartDate = mr.ExpectedStartDate,
                ExpectedEndDate = mr.ExpectedEndDate
            });

            return await PagedList<MaintenanceRequestDTO>.CreateAsync(maintenanceRequestDTOs, query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}