using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Delete
{
    public class DeleteFacilityHandler : ICommandHandler<DeleteFacilityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFacilityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
        {
            var facility = await _unitOfWork.Facilities.GetByIdAsync(request.Id);
            if (facility == null)
                return Response.FailureResponse("Facility not found.");

            await _unitOfWork.Facilities.DeleteAsync(request.Id);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}