using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Queries.GetDemolitionRequests
{
    public class GetDemolitionRequestsHandler : IRequestHandler<GetDemolitionRequestsQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetDemolitionRequestsHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetDemolitionRequestsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.DemolitionRequests.GetAllAsync(cancellationToken);
            var result = entities.Select(e => new KeyValuPair { Key = e.Id, Value = e.Reason });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
} 