using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.BuildingDetailRequests;

namespace SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Create
{
    public class CreateBuildingDetailRequestHandler : ICommandHandler<CreateBuildingDetailRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBuildingDetailRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateBuildingDetailRequestCommand request, CancellationToken cancellationToken)
        {
            var buildingDetailRequest = BuildingDetailRequest.Create(request.RequestDetails, request.RequestDate, request.Id);

            await _unitOfWork.BuildingDetailRequests.AddAsync(buildingDetailRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}