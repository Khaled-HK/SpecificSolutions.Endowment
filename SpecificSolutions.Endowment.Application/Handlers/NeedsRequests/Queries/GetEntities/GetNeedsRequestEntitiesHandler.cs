using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Queries.GetEntities
{
    public class GetNeedsRequestEntitiesHandler : IQueryHandler<GetNeedsRequestEntitiesQuery, IEnumerable<KeyValuPair>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNeedsRequestEntitiesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetNeedsRequestEntitiesQuery query, CancellationToken cancellationToken)
        {
            var needsRequests = await _unitOfWork.NeedsRequests.GetAllAsync(cancellationToken);

            if (!needsRequests.Any())
                return Response.FilterResponse<IEnumerable<KeyValuPair>>(new List<KeyValuPair>());

            var keyValuePairs = needsRequests.Select(nr => new KeyValuPair { Key = nr.Id, Value = nr.NeedsType });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(keyValuePairs);
        }
    }
} 