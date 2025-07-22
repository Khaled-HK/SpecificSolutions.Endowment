using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.FacilityDetails;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Queries.Filter
{
    public class FilterFacilityDetailHandler : IQueryHandler<FilterFacilityDetailQuery, PagedList<FacilityDetailDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterFacilityDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<FacilityDetailDTO>>> Handle(FilterFacilityDetailQuery request, CancellationToken cancellationToken)
        {
            var facilityDetails = await _unitOfWork.FacilityDetails.GetByFilterAsync(request, cancellationToken);

            if (!facilityDetails.Items.Any())
                return Response.FilterResponse(PagedList<FacilityDetailDTO>.Empty());

            return Response.FilterResponse(facilityDetails);
        }
    }
}