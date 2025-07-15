using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Accounts;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Accounts.Queries.Filter
{
    //TODO use iQuere
    public class FilterAccountHandler : IQueryHandler<FilterAccountQuery, PagedList<FilterAccountDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterAccountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<PagedList<FilterAccountDTO>>> Handle(FilterAccountQuery query, CancellationToken cancellationToken)
        {
            PagedList<FilterAccountDTO> requests = await _unitOfWork.Accounts.GetByFilterAsync(query, cancellationToken);

            if (!requests.Items.Any())
            {
                return Response.FilterResponse(PagedList<FilterAccountDTO>.Empty());
            }

            return Response.FilterResponse(requests);
        }
    }
}