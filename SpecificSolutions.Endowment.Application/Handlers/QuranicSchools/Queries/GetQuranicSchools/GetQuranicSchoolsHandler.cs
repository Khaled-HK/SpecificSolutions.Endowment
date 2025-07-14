using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Queries.GetQuranicSchools
{
    public class GetQuranicSchoolsHandler : IRequestHandler<GetQuranicSchoolsQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetQuranicSchoolsHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetQuranicSchoolsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.QuranicSchools.GetAllAsync(cancellationToken);
            var result = entities.Select(e => new KeyValuPair { Key = e.Id, Value = e.Building.Name });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
} 