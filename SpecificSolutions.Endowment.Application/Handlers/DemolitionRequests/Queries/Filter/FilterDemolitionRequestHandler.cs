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
            var DemolitionRequests = await _unitOfWork.DemolitionRequests.GetAllAsync();
            var filteDemolitionRequests = DemolitionRequests
                .Where(drr => drr.ContractorName.Contains(request.SearchTerm))
                .Select(drr => new FilterDemolitionRequestDTO
                {
                    Id = drr.Id,
                    Reason = drr.Reason,
                });

            var pagedList = await PagedList<FilterDemolitionRequestDTO>.CreateAsync(filteDemolitionRequests.AsQueryable(), request.PageNumber, request.PageSize, cancellationToken);

            return new EndowmentResponse<PagedList<FilterDemolitionRequestDTO>>(pagedList);
        }
    }
}