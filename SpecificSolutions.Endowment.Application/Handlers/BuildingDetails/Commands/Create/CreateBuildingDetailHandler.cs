using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.BuildingDetails;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Create
{
    public class CreateBuildingDetailHandler : ICommandHandler<CreateBuildingDetailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBuildingDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateBuildingDetailCommand request, CancellationToken cancellationToken)
        {
            var buildingDetail = BuildingDetail.Create(request);

            await _unitOfWork.BuildingDetails.AddAsync(buildingDetail);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}