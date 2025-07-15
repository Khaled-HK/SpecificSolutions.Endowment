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
            var changeOfPathRequests = await _unitOfWork.ChangeOfPathRequests.GetAllAsync(cancellationToken);
            var filteredRequests = changeOfPathRequests
                .Where(cpr => cpr.CurrentType.Contains(request.SearchTerm) || cpr.NewType.Contains(request.SearchTerm))
                .Select(cpr => new ChangeOfPathRequestDTO
                {
                    Id = cpr.Id,
                    CurrentType = cpr.CurrentType,
                    NewType = cpr.NewType,
                    Reason = cpr.Reason
                });

            var pagedList = await PagedList<ChangeOfPathRequestDTO>.CreateAsync(filteredRequests.AsQueryable(), request.PageNumber, request.PageSize, cancellationToken);

            return Response.FilterResponse(pagedList);
        }
    }
}