using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Delete
{
    public class DeleteFacilityDetailHandler : ICommandHandler<DeleteFacilityDetailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFacilityDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteFacilityDetailCommand request, CancellationToken cancellationToken)
        {
            var facilityDetail = await _unitOfWork.FacilityDetails.GetByIdAsync(request.Id, cancellationToken);
            if (facilityDetail == null)
                return Response.FailureResponse("FacilityDetail not found.");

            await _unitOfWork.FacilityDetails.DeleteAsync(request.Id);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}