using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.NameChangeRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Queries.Filter
{
    public class FilterNameChangeRequestHandler : IQueryHandler<FilterNameChangeRequestQuery, PagedList<NameChangeRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterNameChangeRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<NameChangeRequestDTO>>> Handle(FilterNameChangeRequestQuery request, CancellationToken cancellationToken)
        {
            var pagedList = await _unitOfWork.NameChangeRequests.GetByFilterAsync(request, cancellationToken);

            return Response.FilterResponse(pagedList);
        }
    }
}