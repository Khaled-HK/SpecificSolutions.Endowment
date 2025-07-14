using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.ConstructionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Queries.GetConstructionRequest
{
    public class GetConstructionRequestHandler : IRequestHandler<GetConstructionRequestQuery, EndowmentResponse<ConstructionRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetConstructionRequestHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<ConstructionRequestDTO>> Handle(GetConstructionRequestQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.ConstructionRequests.FindAsync(request.Id, cancellationToken);
            if (entity == null)
                return Response.FailureResponse<ConstructionRequestDTO>("The specified construction request could not be located. Please verify the ID and try again.");
            var dto = new ConstructionRequestDTO
            {
                Id = entity.Id,
                BuildingType = "Standard", // Default value since it's not in entity
                ProposedLocation = entity.ProposedLocation,
                ProposedArea = entity.ProposedArea,
                EstimatedCost = entity.EstimatedCost,
                ContractorName = entity.ContractorName
            };
            return new(data: dto);
        }
    }
} 