using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.DemolitionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Queries.Filter
{
    public class FilterDemolitionRequestHandler : IQueryHandler<FilterDemolitionRequestQuery, PagedList<FilterDemolitionRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterDemolitionRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<FilterDemolitionRequestDTO>>> Handle(FilterDemolitionRequestQuery request, CancellationToken cancellationToken)
        {
            var pagedList = await _unitOfWork.DemolitionRequests.GetByFilterAsync(request, cancellationToken);

            return Response.FilterResponse(pagedList);
        }
    }
}