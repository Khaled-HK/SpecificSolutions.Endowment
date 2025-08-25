using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.DemolitionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.DemolitionRequests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.DemolitionRequests
{
    public class DemolitionRequestRepository : Repository<DemolitionRequest>, IDemolitionRequestRepository
    {
        private readonly AppDbContext _context;

        public DemolitionRequestRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<DemolitionRequest> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.DemolitionRequests.FindAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<DemolitionRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.DemolitionRequests.ToListAsync(cancellationToken);
        }

        public async Task<PagedList<FilterDemolitionRequestDTO>> GetByFilterAsync(FilterDemolitionRequestQuery query, CancellationToken cancellationToken)
        {
            var demolitionRequests = _context.DemolitionRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                demolitionRequests = demolitionRequests.Where(dr =>
                    dr.ContractorName.Contains(query.SearchTerm) ||
                    dr.Reason.Contains(query.SearchTerm) ||
                    dr.Location.Contains(query.SearchTerm) ||
                    (dr.Request != null && (
                        dr.Request.Title.Contains(query.SearchTerm) ||
                        dr.Request.Description.Contains(query.SearchTerm) ||
                        dr.Request.ReferenceNumber.Contains(query.SearchTerm)
                    ))
                );
            }

            var demolitionRequestDTOs = demolitionRequests.Select(dr => new FilterDemolitionRequestDTO
            {
                Id = dr.Id,
                // Base FilterRequestDTO properties (from Request)
                Title = dr.Request != null ? dr.Request.Title : null,
                Description = dr.Request != null ? dr.Request.Description : null,
                CreatedDate = dr.Request != null ? dr.Request.CreatedDate : default,
                ReferenceNumber = dr.Request != null ? dr.Request.ReferenceNumber : null,
                SubmissionDate = dr.Request != null ? dr.Request.CreatedDate : default,
                Priority = "Medium", // Default value
                Location = dr.Location,
                RequestStatus = "Pending", // Default value
                Attachments = new List<string>(), // Default empty list
                // Specific Demolition fields
                DemolitionReason = dr.Reason,
                Reason = dr.Reason,
                ContractorName = dr.ContractorName,
                EstimatedRebuildingCost = (decimal)dr.EstimatedReconstructionCost,
                EstimatedCost = dr.EstimatedReconstructionCost.ToString(),
                EstimatedTime = "30 days", // Default value
                BuildingType = "Residential", // Default value
                SafetyMeasures = "Standard safety protocols" // Default value
            });

            return await PagedList<FilterDemolitionRequestDTO>.CreateAsync(
                demolitionRequestDTOs,
                query.PageNumber,
                query.PageSize,
                cancellationToken
            );
        }
    }
}