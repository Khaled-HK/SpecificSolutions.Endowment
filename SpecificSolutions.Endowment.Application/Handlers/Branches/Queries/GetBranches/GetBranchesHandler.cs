using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Branches.Queries.GetBranches
{
    public class GetBranchesHandler : IQueryHandler<GetBranchesQuery, IEnumerable<KeyValuPair>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetBranchesHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetBranchesQuery query, CancellationToken cancellationToken)
        {
            var branches = await _unitOfWork.Branches.GetAllAsync(cancellationToken);
            var keyValuePairs = branches.Select(b => new KeyValuPair { Key = b.Id, Value = b.Name });
            return Response.FilterResponse(keyValuePairs);
        }
    }
} 