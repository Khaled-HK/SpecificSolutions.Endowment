using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Update
{
    public class UpdateBuildingHandler : ICommandHandler<UpdateBuildingCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBuildingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = await _unitOfWork.Buildings.FindAsync(request.Id, cancellationToken);

            if (building == null)
                return Response.FailureResponse("Id", "Building not found.");

            building.Update(request);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}