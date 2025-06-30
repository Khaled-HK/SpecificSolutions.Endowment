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
            var building = await _unitOfWork.BuildingDetails.FindAsync(request.Id);
            if (building == null)
            {
                return Response.FailureResponse("Id", "BuildingDetails not found.");
            }

            building.Update(request.Name);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}