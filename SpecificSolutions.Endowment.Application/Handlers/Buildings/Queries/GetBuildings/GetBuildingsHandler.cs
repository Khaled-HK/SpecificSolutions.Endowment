using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Buildings.Queries.GetBuildings
{
    public class GetBuildingsHandler : IRequestHandler<GetBuildingsQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetBuildingsHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetBuildingsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Buildings.GetAllAsync(cancellationToken);
            var result = entities.Select(e => new KeyValuPair { Key = e.Id, Value = e.Name });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
} 