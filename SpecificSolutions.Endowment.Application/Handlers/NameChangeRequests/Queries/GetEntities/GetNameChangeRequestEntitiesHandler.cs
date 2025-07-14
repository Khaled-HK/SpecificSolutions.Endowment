using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Queries.GetEntities
{
    public class GetNameChangeRequestEntitiesHandler : IQueryHandler<GetNameChangeRequestEntitiesQuery, IEnumerable<KeyValuPair>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNameChangeRequestEntitiesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetNameChangeRequestEntitiesQuery query, CancellationToken cancellationToken)
        {
            var nameChangeRequests = await _unitOfWork.NameChangeRequests.GetAllAsync(cancellationToken);

            if (!nameChangeRequests.Any())
                return Response.FilterResponse<IEnumerable<KeyValuPair>>(new List<KeyValuPair>());

            var keyValuePairs = nameChangeRequests.Select(ncr => new KeyValuPair { Key = ncr.Id, Value = ncr.CurrentName });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(keyValuePairs);
        }
    }
} 