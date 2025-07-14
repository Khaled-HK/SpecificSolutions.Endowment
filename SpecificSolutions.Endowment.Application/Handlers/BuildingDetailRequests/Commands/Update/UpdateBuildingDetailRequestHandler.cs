using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Update
{
    public class UpdateBuildingDetailRequestHandler : ICommandHandler<UpdateBuildingDetailRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBuildingDetailRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateBuildingDetailRequestCommand request, CancellationToken cancellationToken)
        {
            var buildingDetailRequest = await _unitOfWork.BuildingDetailRequests.GetByIdAsync(request.Id, cancellationToken);
            if (buildingDetailRequest == null)
                return Response.FailureResponse("BuildingDetailRequest not found.");

            await _unitOfWork.BuildingDetailRequests.UpdateAsync(buildingDetailRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}