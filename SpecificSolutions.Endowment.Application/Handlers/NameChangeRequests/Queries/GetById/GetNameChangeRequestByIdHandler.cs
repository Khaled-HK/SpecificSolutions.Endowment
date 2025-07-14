using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.NameChangeRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Queries.GetById
{
    public class GetNameChangeRequestByIdHandler : IRequestHandler<GetNameChangeRequestByIdQuery, EndowmentResponse<NameChangeRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNameChangeRequestByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<NameChangeRequestDTO>> Handle(GetNameChangeRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var nameChangeRequest = await _unitOfWork.NameChangeRequests.FindAsync(request.Id, cancellationToken: cancellationToken);
            if (nameChangeRequest == null)
            {
                return Response.FailureResponse<NameChangeRequestDTO>("The specified Name Change Request could not be located. Please verify the Name Change Request ID and try again.");
            }

            var nameChangeRequestDTO = new NameChangeRequestDTO
            {
                Id = nameChangeRequest.Id,
                CurrentName = nameChangeRequest.CurrentName,
                NewName = nameChangeRequest.NewName,
                Reason = nameChangeRequest.Reason
            };

            return new(data: nameChangeRequestDTO);
        }
    }
} 