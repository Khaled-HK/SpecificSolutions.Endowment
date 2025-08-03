using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Buildings;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Buildings.Queries.Filter
{
    public class FilterBuildingHandler : IQueryHandler<FilterBuildingQuery, PagedList<BuildingDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterBuildingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<BuildingDTO>>> Handle(FilterBuildingQuery request, CancellationToken cancellationToken)
        {
            var pagedList = await _unitOfWork.Buildings.GetByFilterAsync(request, cancellationToken);

            return Response.FilterResponse(pagedList);
        }
    }
}