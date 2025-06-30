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
            var constructionRequests = await _unitOfWork.ConstructionRequests.GetAllAsync();
            var filteredRequests = constructionRequests
                .Where(cr => cr.ProposedLocation.Contains(request.SearchTerm))
                .Select(cr => new ConstructionRequestDTO
                {
                    Id = cr.Id,
                    ProposedLocation = cr.ProposedLocation,
                    ProposedArea = cr.ProposedArea,
                    EstimatedCost = cr.EstimatedCost,
                    ContractorName = cr.ContractorName
                });

            var pagedList = await PagedList<ConstructionRequestDTO>.CreateAsync(filteredRequests.AsQueryable(), request.PageNumber, request.PageSize, cancellationToken);

            return new EndowmentResponse<PagedList<ConstructionRequestDTO>>(pagedList);
        }
    }
}