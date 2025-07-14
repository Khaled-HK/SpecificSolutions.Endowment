using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.ChangeOfPathRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Queries.GetChangeOfPathRequest
{
    public class GetChangeOfPathRequestHandler : IRequestHandler<GetChangeOfPathRequestQuery, EndowmentResponse<ChangeOfPathRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetChangeOfPathRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<ChangeOfPathRequestDTO>> Handle(GetChangeOfPathRequestQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.ChangeOfPathRequests.FindAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                return Response.FailureResponse<ChangeOfPathRequestDTO>("The specified change of path request could not be located. Please verify the ID and try again.");
            }

            var dto = new ChangeOfPathRequestDTO
            {
                Id = entity.Id,
                CurrentType = entity.CurrentType,
                NewType = entity.NewType,
                Reason = entity.Reason
            };

            return new(data: dto);
        }
    }
} 