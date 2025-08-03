using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Facilities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Facilities.Queries.Filter
{
    public class FilterFacilityHandler : IQueryHandler<FilterFacilityQuery, PagedList<FacilityDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterFacilityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<FacilityDTO>>> Handle(FilterFacilityQuery request, CancellationToken cancellationToken)
        {
            var pagedList = await _unitOfWork.Facilities.GetByFilterAsync(request, cancellationToken);

            return Response.FilterResponse(pagedList);
        }
    }
}