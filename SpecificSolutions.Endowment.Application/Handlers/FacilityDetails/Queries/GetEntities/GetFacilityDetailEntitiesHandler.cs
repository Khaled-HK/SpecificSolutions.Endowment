using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Queries.GetEntities
{
    public class GetFacilityDetailEntitiesHandler : IQueryHandler<GetFacilityDetailEntitiesQuery, IEnumerable<KeyValuPair>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFacilityDetailEntitiesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetFacilityDetailEntitiesQuery query, CancellationToken cancellationToken)
        {
            var facilityDetails = await _unitOfWork.FacilityDetails.GetAllAsync(cancellationToken);

            if (!facilityDetails.Any())
                return Response.FilterResponse<IEnumerable<KeyValuPair>>(new List<KeyValuPair>());

            var keyValuePairs = facilityDetails.Select(fd => new KeyValuPair { Key = fd.Id/*, Value = fd.Name*/ });
            return Response.FilterResponse(keyValuePairs);
        }
    }
}