using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Requests.Queries.GetById
{
    public class GetRequestByIdHandler : IRequestHandler<GetRequestByIdQuery, EndowmentResponse<FilterRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRequestByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<FilterRequestDTO>> Handle(GetRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var requestEntity = await _unitOfWork.Requests.GetByIdAsync(request.Id, cancellationToken);
            if (requestEntity == null)
            {
                return Response.FailureResponse<FilterRequestDTO>("The specified Request could not be located. Please verify the Request ID and try again.");
            }

            var requestDTO = new FilterRequestDTO
            {
                Id = requestEntity.Id,
                Title = requestEntity.Title,
                Description = requestEntity.Description,
                ReferenceNumber = requestEntity.ReferenceNumber
            };

            return new(data: requestDTO);
        }
    }
} 