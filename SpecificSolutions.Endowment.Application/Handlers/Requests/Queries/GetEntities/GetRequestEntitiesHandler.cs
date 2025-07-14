using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Requests.Queries.GetEntities
{
    public class GetRequestEntitiesHandler : IQueryHandler<GetRequestEntitiesQuery, IEnumerable<KeyValuPair>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRequestEntitiesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetRequestEntitiesQuery query, CancellationToken cancellationToken)
        {
            var requests = await _unitOfWork.Requests.GetAllAsync(cancellationToken);

            if (!requests.Any())
                return Response.FilterResponse<IEnumerable<KeyValuPair>>(new List<KeyValuPair>());

            var keyValuePairs = requests.Select(r => new KeyValuPair { Key = r.Id, Value = r.Title });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(keyValuePairs);
        }
    }
} 