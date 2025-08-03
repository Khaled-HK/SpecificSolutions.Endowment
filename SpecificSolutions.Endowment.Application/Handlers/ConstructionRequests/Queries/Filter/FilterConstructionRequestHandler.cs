using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.ConstructionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Queries.Filter
{
    public class FilterConstructionRequestHandler : IQueryHandler<FilterConstructionRequestQuery, PagedList<ConstructionRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterConstructionRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<ConstructionRequestDTO>>> Handle(FilterConstructionRequestQuery request, CancellationToken cancellationToken)
        {
            var pagedList = await _unitOfWork.ConstructionRequests.GetByFilterAsync(request, cancellationToken);

            return Response.FilterResponse(pagedList);
        }
    }
}