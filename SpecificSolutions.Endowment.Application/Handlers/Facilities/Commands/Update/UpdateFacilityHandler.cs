using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Update
{
    public class UpdateFacilityHandler : ICommandHandler<UpdateFacilityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFacilityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateFacilityCommand request, CancellationToken cancellationToken)
        {
            var facility = await _unitOfWork.Facilities.GetByIdAsync(request.Id, cancellationToken);
            if (facility == null)
                return Response.FailureResponse("Facility not found.");

            await _unitOfWork.Facilities.UpdateAsync(facility);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}