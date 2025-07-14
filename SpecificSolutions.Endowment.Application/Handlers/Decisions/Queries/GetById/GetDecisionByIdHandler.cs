using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Decisions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.GetById
{
    public class GetDecisionByIdHandler : IRequestHandler<GetDecisionByIdQuery, EndowmentResponse<FilterDecisionDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDecisionByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<FilterDecisionDTO>> Handle(GetDecisionByIdQuery request, CancellationToken cancellationToken)
        {
            var decision = await _unitOfWork.Decisions.GetByIdAsync(request.Id, cancellationToken);
            if (decision == null)
            {
                return Response.FailureResponse<FilterDecisionDTO>("The specified Decision could not be located. Please verify the Decision ID and try again.");
            }

            var decisionDTO = new FilterDecisionDTO
            {
                Id = decision.Id,
                Title = decision.Title,
                Description = decision.Description,
                CreatedDate = decision.CreatedDate,
                ReferenceNumber = decision.ReferenceNumber
            };

            return new(data: decisionDTO);
        }
    }
} 