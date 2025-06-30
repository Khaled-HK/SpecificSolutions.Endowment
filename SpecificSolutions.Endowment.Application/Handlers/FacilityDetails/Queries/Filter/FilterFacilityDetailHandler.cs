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
            var FacilityDetails = await _unitOfWork.FacilityDetails.GetAllAsync();
            var filteredDetails = FacilityDetails
                .Where(md => md.Product.Name.Contains(request.SearchTerm))
                .Select(md => new FacilityDetailDTO
                {
                    Id = md.Id,

                });

            var pagedList = await PagedList<FacilityDetailDTO>.CreateAsync(filteredDetails.AsQueryable(), request.PageNumber, request.PageSize, cancellationToken);

            return new EndowmentResponse<PagedList<FacilityDetailDTO>>(pagedList);
        }
    }
}