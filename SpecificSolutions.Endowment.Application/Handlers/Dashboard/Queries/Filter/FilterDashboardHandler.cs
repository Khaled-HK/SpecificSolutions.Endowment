using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Dashboard;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Dashboard.Queries.Filter
{
    public sealed class FilterDashboardHandler : IQueryHandler<FilterDashboardQuery, DashboardSummaryDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterDashboardHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<DashboardSummaryDTO>> Handle(FilterDashboardQuery request, CancellationToken cancellationToken)
        {
            var dto = await _unitOfWork.Dashboard.GetByFilterAsync(request, cancellationToken);
            return Response.FilterResponse(dto);
        }
    }
}


