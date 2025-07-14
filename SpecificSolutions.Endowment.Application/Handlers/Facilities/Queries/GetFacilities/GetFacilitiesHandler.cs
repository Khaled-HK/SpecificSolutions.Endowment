using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Facilities.Queries.GetFacilities
{
    public class GetFacilitiesHandler : IRequestHandler<GetFacilitiesQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetFacilitiesHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetFacilitiesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Facilities.GetAllAsync(cancellationToken);
            var result = entities.Select(e => new KeyValuPair { Key = e.Id, Value = e.Name });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
} 