using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.NeedsRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Queries.GetById
{
    public class GetNeedsRequestByIdHandler : IRequestHandler<GetNeedsRequestByIdQuery, EndowmentResponse<NeedsRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNeedsRequestByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<NeedsRequestDTO>> Handle(GetNeedsRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var needsRequest = await _unitOfWork.NeedsRequests.FindAsync(request.NeedsRequestID, cancellationToken: cancellationToken);
            if (needsRequest == null)
            {
                return Response.FailureResponse<NeedsRequestDTO>("The specified Needs Request could not be located. Please verify the Needs Request ID and try again.");
            }

            var needsRequestDTO = new NeedsRequestDTO
            {
                Id = needsRequest.Id,
                NeedsType = needsRequest.NeedsType,
                Location = needsRequest.Location,
                EstimatedCost = needsRequest.EstimatedCost,
                Provider = needsRequest.Provider,
                RequestId = needsRequest.RequestId
            };

            return new(data: needsRequestDTO);
        }
    }
} 