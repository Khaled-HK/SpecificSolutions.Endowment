using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.FacilityDetails;

namespace SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Create
{
    public class CreateFacilityDetailHandler : ICommandHandler<CreateFacilityDetailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFacilityDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateFacilityDetailCommand request, CancellationToken cancellationToken)
        {
            var facilityDetail = FacilityDetail.Create(request);

            await _unitOfWork.FacilityDetails.AddAsync(facilityDetail);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}