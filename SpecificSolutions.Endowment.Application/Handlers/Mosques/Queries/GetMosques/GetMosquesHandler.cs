using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.GetMosques
{
    public class GetMosquesHandler : IRequestHandler<GetMosquesQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetMosquesHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetMosquesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Mosques.GetAllAsync(cancellationToken);
            var result = entities.Select(e => new KeyValuPair { Key = e.Id, Value = e.Building.Name });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
} 