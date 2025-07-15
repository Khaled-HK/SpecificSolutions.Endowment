using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Decisions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.Filter
{
    public class FilterDecisionHandler : IQueryHandler<FilterDecisionQuery, PagedList<FilterDecisionDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterDecisionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<FilterDecisionDTO>>> Handle(FilterDecisionQuery query, CancellationToken cancellationToken)
        {
            var pagedList = await _unitOfWork.Decisions.GetByFilterAsync(query, cancellationToken);

            return Response.FilterResponse(pagedList);
        }
    }
}