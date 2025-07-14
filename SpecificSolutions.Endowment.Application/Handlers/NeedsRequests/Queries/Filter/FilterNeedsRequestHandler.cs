using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.NeedsRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Queries.Filter
{
    public class FilterNeedsRequestHandler : IQueryHandler<FilterNeedsRequestQuery, PagedList<FilterNeedsRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterNeedsRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<FilterNeedsRequestDTO>>> Handle(FilterNeedsRequestQuery request, CancellationToken cancellationToken)
        {
            var needsRequests = await _unitOfWork.NeedsRequests.GetAllAsync(cancellationToken);
            var filteredRequests = needsRequests
                .Where(nr => nr.NeedsType.Contains(request.SearchTerm))
                .Select(nr => new FilterNeedsRequestDTO
                {
                    Id = nr.Id,
                });

            var pagedList = await PagedList<FilterNeedsRequestDTO>.CreateAsync(filteredRequests.AsQueryable(), request.PageNumber, request.PageSize, cancellationToken);
            return new EndowmentResponse<PagedList<FilterNeedsRequestDTO>>(pagedList);
        }
    }
}