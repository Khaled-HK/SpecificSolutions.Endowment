using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Delete
{
    public class DeleteBuildingDetailRequestHandler : ICommandHandler<DeleteBuildingDetailRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBuildingDetailRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteBuildingDetailRequestCommand request, CancellationToken cancellationToken)
        {
            var buildingDetailRequest = await _unitOfWork.BuildingDetailRequests.GetByIdAsync(request.Id);
            if (buildingDetailRequest == null)
                return Response.FailureResponse("BuildingDetailRequest not found.");

            await _unitOfWork.BuildingDetailRequests.DeleteAsync(request.Id);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}