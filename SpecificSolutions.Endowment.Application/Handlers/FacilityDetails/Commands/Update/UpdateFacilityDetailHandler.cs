using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Update
{
    public class UpdateFacilityDetailHandler : ICommandHandler<UpdateFacilityDetailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFacilityDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateFacilityDetailCommand request, CancellationToken cancellationToken)
        {
            var facilityDetail = await _unitOfWork.FacilityDetails.GetByIdAsync(request.Id, cancellationToken);
            if (facilityDetail == null)
                return Response.FailureResponse("FacilityDetail not found.");

            await _unitOfWork.FacilityDetails.UpdateAsync(facilityDetail);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}