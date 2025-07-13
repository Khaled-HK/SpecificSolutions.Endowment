using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.GetDecisions
{
    public class GetDecisionsHandler : IQueryHandler<GetDecisionsQuery, IEnumerable<KeyValuPair>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDecisionsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetDecisionsQuery query, CancellationToken cancellationToken)
        {
            var Decisions = await _unitOfWork.Decisions.GetDecisionsAsync(query, cancellationToken);
            return new EndowmentResponse<IEnumerable<KeyValuPair>>(Decisions);
        }
    }
}
