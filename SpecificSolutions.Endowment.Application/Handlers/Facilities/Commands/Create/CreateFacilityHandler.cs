using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Facilities;

namespace SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Create
{
    public class CreateFacilityHandler : ICommandHandler<CreateFacilityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFacilityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateFacilityCommand request, CancellationToken cancellationToken)
        {
            var facility = Facility.Create(
                request.Name,
                request.Location,
                request.ContactInfo,
                request.Capacity,
                request.Status
            );

            await _unitOfWork.Facilities.AddAsync(facility);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}