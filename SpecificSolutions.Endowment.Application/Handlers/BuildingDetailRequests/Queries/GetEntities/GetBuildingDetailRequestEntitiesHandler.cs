using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Queries.GetEntities
{
    public class GetBuildingDetailRequestEntitiesHandler : IQueryHandler<GetBuildingDetailRequestEntitiesQuery, IEnumerable<KeyValuPair>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBuildingDetailRequestEntitiesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetBuildingDetailRequestEntitiesQuery query, CancellationToken cancellationToken)
        {
            var buildingDetailRequests = await _unitOfWork.BuildingDetailRequests.GetAllAsync(cancellationToken);

            if (!buildingDetailRequests.Any())
                return Response.FilterResponse<IEnumerable<KeyValuPair>>(new List<KeyValuPair>());

            var keyValuePairs = buildingDetailRequests.Select(bdr => new KeyValuPair { Key = bdr.Id, Value = bdr.RequestDetails });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(keyValuePairs);
        }
    }
} 