using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Update
{
    public class UpdateBuildingDetailHandler : ICommandHandler<UpdateBuildingDetailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBuildingDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateBuildingDetailCommand request, CancellationToken cancellationToken)
        {
            var buildingDetail = await _unitOfWork.BuildingDetails.GetByIdAsync(request.Id, cancellationToken);
            if (buildingDetail == null)
            {
                return Response.FailureResponse("Building detail not found.");
            }

            buildingDetail.Update(request);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}