using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Banks.Queries.GetBanks
{
    public class GetBanksHandler : IQueryHandler<GetBanksQuery, IEnumerable<KeyValuPair>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBanksHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetBanksQuery query, CancellationToken cancellationToken)
        {
            var banks = await _unitOfWork.Banks.GetAllAsync(cancellationToken);

            if (!banks.Any())
                return Response.FilterResponse<IEnumerable<KeyValuPair>>(new List<KeyValuPair>());

            var keyValuePairs = banks.Select(b => new KeyValuPair { Key = b.Id, Value = b.Name });
            return Response.FilterResponse(keyValuePairs);
        }
    }
}