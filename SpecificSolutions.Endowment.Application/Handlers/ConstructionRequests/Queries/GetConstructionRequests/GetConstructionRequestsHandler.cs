using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Queries.GetConstructionRequests
{
    public class GetConstructionRequestsHandler : IRequestHandler<GetConstructionRequestsQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetConstructionRequestsHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetConstructionRequestsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.ConstructionRequests.GetAllAsync(cancellationToken);
            var result = entities.Select(e => new KeyValuPair { Key = e.Id, Value = e.ProposedLocation });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
} 