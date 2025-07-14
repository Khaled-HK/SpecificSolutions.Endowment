using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.GetEntities
{
    public class GetEntitiesHandler : IRequestHandler<GetEntitiesQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetEntitiesHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetEntitiesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Decisions.GetAllAsync(cancellationToken);
            var result = entities.Select(e => new KeyValuPair { Key = e.Id, Value = e.Title });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
} 