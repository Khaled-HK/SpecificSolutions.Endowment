using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.ExpenditureChangeRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Queries.Filter
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
            var expenditureChangeRequests = await _unitOfWork.ExpenditureChangeRequests.GetAllAsync();
            var filteredRequests = expenditureChangeRequests
                .Where(ecr => ecr.Reason.Contains(request.SearchTerm))
                .Select(ecr => new ExpenditureChangeRequestDTO
                {
                    Id = ecr.Id,
                    Reason = ecr.Reason,
                });

            var pagedList = await PagedList<ExpenditureChangeRequestDTO>.CreateAsync(filteredRequests.AsQueryable(), request.PageNumber, request.PageSize, cancellationToken);

            return new EndowmentResponse<PagedList<ExpenditureChangeRequestDTO>>(pagedList);
        }
    }
}