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
            var decision = await _unitOfWork.BuildingDetails.GetByIdAsync(request.Id);
            if (decision == null)
            {
                return Response.FailureResponse("Id", "BuildingDetail not found.");
            }

            await _unitOfWork.BuildingDetails.RemoveAsync(decision);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}