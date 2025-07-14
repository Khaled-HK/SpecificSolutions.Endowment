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
            var nameChangeRequests = await _unitOfWork.NameChangeRequests.GetAllAsync(cancellationToken);
            var filteredRequests = nameChangeRequests
                 .Where(ncr => ncr.CurrentName.Contains(request.SearchTerm) || ncr.NewName.Contains(request.SearchTerm))
                 .Select(ncr => new NameChangeRequestDTO
                 {
                     Id = ncr.Id,
                     CurrentName = ncr.CurrentName,
                     NewName = ncr.NewName,
                     Reason = ncr.Reason
                 });

            var pagedList = await PagedList<NameChangeRequestDTO>.CreateAsync(filteredRequests.AsQueryable(), request.PageNumber, request.PageSize, cancellationToken);

            return new EndowmentResponse<PagedList<NameChangeRequestDTO>>(pagedList);
        }
    }
}