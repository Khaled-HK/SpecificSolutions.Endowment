using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.ExpenditureChangeRequests;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Application.Handlers.ExpenditureChangeRequests.Queries.Filter;

namespace SpecificSolutions.Endowment.Application.Handlers.ExpenditureChangeRequests.Queries.Filter
{
    public class FilterExpenditureChangeRequestHandler : IQueryHandler<FilterExpenditureChangeRequestQuery, PagedList<ExpenditureChangeRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterExpenditureChangeRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<ExpenditureChangeRequestDTO>>> Handle(FilterExpenditureChangeRequestQuery request, CancellationToken cancellationToken)
        {
            var pagedList = await _unitOfWork.ExpenditureChangeRequests.GetByFilterAsync(request, cancellationToken);

            return Response.FilterResponse(pagedList);
        }
    }
}