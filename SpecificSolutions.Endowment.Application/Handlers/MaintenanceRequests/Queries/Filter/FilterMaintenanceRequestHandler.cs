using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.MaintenanceRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Queries.Filter
{
    public class FilterMaintenanceRequestHandler : IQueryHandler<FilterMaintenanceRequestQuery, PagedList<MaintenanceRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterMaintenanceRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<MaintenanceRequestDTO>>> Handle(FilterMaintenanceRequestQuery request, CancellationToken cancellationToken)
        {
            var maintenanceRequests = await _unitOfWork.MaintenanceRequests.GetAllAsync(cancellationToken);
            var filteredRequests = maintenanceRequests
                   .Where(mr => mr.Location.Contains(request.SearchTerm))
                   .Select(mr => new MaintenanceRequestDTO
                   {
                       Id = mr.Id,
                       MaintenanceType = mr.MaintenanceType,
                       Location = mr.Location,
                       EstimatedCost = mr.EstimatedCost,
                       ExpectedStartDate = mr.ExpectedStartDate,
                       ExpectedEndDate = mr.ExpectedEndDate
                   });

            var pagedList = await PagedList<MaintenanceRequestDTO>.CreateAsync(filteredRequests.AsQueryable(), request.PageNumber, request.PageSize, cancellationToken);

            return Response.FilterResponse(pagedList);

        }
    }
}