using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.DemolitionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Queries.GetDemolitionRequest
{
    public class GetDemolitionRequestHandler : IRequestHandler<GetDemolitionRequestQuery, EndowmentResponse<DemolitionRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetDemolitionRequestHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<DemolitionRequestDTO>> Handle(GetDemolitionRequestQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.DemolitionRequests.FindAsync(request.Id, cancellationToken);
            if (entity == null)
                return Response.FailureResponse<DemolitionRequestDTO>("The specified demolition request could not be located. Please verify the ID and try again.");
            var dto = new DemolitionRequestDTO
            {
                Id = entity.Id,
                Location = entity.Location,
                Reason = entity.Reason,
                ContractorName = entity.ContractorName,
                EstimatedCost = (decimal)entity.EstimatedReconstructionCost,
                EstimatedTime = "To be determined", // Default value since it's not in entity
                EstimatedRebuildingCost = (decimal)entity.EstimatedReconstructionCost,
                Status = "Pending" // Default value since it's not in entity
            };
            return new(data: dto);
        }
    }
} 