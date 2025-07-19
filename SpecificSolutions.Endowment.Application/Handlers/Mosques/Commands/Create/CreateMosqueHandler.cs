using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Mosques;
using SpecificSolutions.Endowment.Core.Entities.Buildings;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Create
{
    public class CreateMosqueHandler : ICommandHandler<CreateMosqueCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMosqueHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateMosqueCommand request, CancellationToken cancellationToken)
        {
            // Create the building first
            var building = Building.Create(request);
            await _unitOfWork.Buildings.AddAsync(building, cancellationToken);
            
            // Create the mosque with the existing building
            var mosque = Mosque.Create(request, building);
            await _unitOfWork.Mosques.AddAsync(mosque, cancellationToken);
            
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}