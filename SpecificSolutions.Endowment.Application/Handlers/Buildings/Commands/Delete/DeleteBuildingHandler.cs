using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Delete
{
    public class DeleteBuildingHandler : ICommandHandler<DeleteBuildingCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBuildingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = await _unitOfWork.Buildings.FindAsync(request.Id);
            if (building == null)
                return Response.FailureResponse("Id", "BuildingDetail not found.");


            await _unitOfWork.Buildings.RemoveAsync(building);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}