using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.ChangeOfPathRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Queries.Filter
{
    public class FilterChangeOfPathRequestHandler : IQueryHandler<FilterChangeOfPathRequestQuery, PagedList<ChangeOfPathRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterChangeOfPathRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<ChangeOfPathRequestDTO>>> Handle(FilterChangeOfPathRequestQuery request, CancellationToken cancellationToken)
        {
            var pagedList = await _unitOfWork.ChangeOfPathRequests.GetByFilterAsync(request, cancellationToken);

            return Response.FilterResponse(pagedList);
        }
    }
}