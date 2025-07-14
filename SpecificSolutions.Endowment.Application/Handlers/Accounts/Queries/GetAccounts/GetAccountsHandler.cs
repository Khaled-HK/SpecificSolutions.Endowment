using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Accounts.Queries.GetAccounts
{
    public class GetAccountsHandler : IQueryHandler<GetAccountsQuery, IEnumerable<KeyValuPair>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAccountsHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetAccountsQuery query, CancellationToken cancellationToken)
        {
            var accounts = await _unitOfWork.Accounts.GetAllAsync(cancellationToken);
            var keyValuePairs = accounts.Select(a => new KeyValuPair { Key = a.Id, Value = a.Name });
            return Response.FilterResponse(keyValuePairs);
        }
    }
} 