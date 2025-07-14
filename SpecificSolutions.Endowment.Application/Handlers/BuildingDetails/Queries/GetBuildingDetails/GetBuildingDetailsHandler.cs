using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Queries.GetBuildingDetails
{
    public class GetBuildingDetailsHandler : IRequestHandler<GetBuildingDetailsQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBuildingDetailsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetBuildingDetailsQuery request, CancellationToken cancellationToken)
        {
            var buildingDetails = await _unitOfWork.BuildingDetails.GetAllAsync(cancellationToken);
            
            var result = buildingDetails.Select(bd => new KeyValuPair
            {
                Key = bd.Id,
                Value = bd.Name
            });

            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
} 