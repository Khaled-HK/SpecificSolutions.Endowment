using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Queries.GetExpenditureChangeRequests
{
    public class GetExpenditureChangeRequestsHandler : IRequestHandler<GetExpenditureChangeRequestsQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetExpenditureChangeRequestsHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetExpenditureChangeRequestsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.ExpenditureChangeRequests.GetAllAsync(cancellationToken);
            var result = entities.Select(e => new KeyValuPair { Key = e.Id, Value = e.Reason });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
} 