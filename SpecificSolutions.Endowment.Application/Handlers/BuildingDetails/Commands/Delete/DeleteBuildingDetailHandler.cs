using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Delete
{
    public class DeleteBuildingDetailHandler : ICommandHandler<DeleteBuildingDetailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBuildingDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteBuildingDetailCommand request, CancellationToken cancellationToken)
        {
            var buildingDetail = await _unitOfWork.BuildingDetails.GetByIdAsync(request.Id, cancellationToken);
            if (buildingDetail == null)
            {
                return Response.FailureResponse("Id", "BuildingDetail not found.");
            }

            await _unitOfWork.BuildingDetails.RemoveAsync(buildingDetail);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}