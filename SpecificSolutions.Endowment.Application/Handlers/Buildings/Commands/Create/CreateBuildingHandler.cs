using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Buildings;

namespace SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Create
{
    public class CreateBuildingHandler : ICommandHandler<CreateBuildingCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBuildingHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateBuildingCommand request, CancellationToken cancellationToken)
        {
            var building = Building.Create(request);

            await _unitOfWork.Buildings.AddAsync(building, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}