using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Branchs;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Branches.Queries.Filter
{
    public class FilterBranchHandler : IQueryHandler<FilterBranchQuery, PagedList<BranchDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterBranchHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<BranchDTO>>> Handle(FilterBranchQuery query, CancellationToken cancellationToken)
        {
            var requests = await _unitOfWork.Branches.GetByFilterAsync(query, cancellationToken);

            if (!requests.Items.Any())
            {
                return Response.FilterResponse(PagedList<BranchDTO>.Empty());
            }

            return Response.FilterResponse(requests);
        }
    }
}