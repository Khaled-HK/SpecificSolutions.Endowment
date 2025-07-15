using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetailRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Queries.Filter
{
    public class FilterBuildingDetailRequestHandler : IQueryHandler<FilterBuildingDetailRequestQuery, PagedList<BuildingDetailRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterBuildingDetailRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<BuildingDetailRequestDTO>>> Handle(FilterBuildingDetailRequestQuery query, CancellationToken cancellationToken)
        {
            var requests = await _unitOfWork.BuildingDetailRequests.GetByFilterAsync(query, cancellationToken);

            if (!requests.Items.Any())
            {
                return Response.FilterResponse(PagedList<BuildingDetailRequestDTO>.Empty());
            }

            return Response.FilterResponse(requests);
        }
    }
}