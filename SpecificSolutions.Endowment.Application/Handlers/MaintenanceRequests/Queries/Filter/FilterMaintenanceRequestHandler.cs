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
            var pagedList = await _unitOfWork.MaintenanceRequests.GetByFilterAsync(request, cancellationToken);

            return Response.FilterResponse(pagedList);
        }
    }
}