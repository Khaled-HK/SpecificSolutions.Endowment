using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.ExpenditureChangeRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Queries.GetById
{
    public class GetByIdHandler : IRequestHandler<GetByIdQuery, EndowmentResponse<ExpenditureChangeRequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetByIdHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<ExpenditureChangeRequestDTO>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.ExpenditureChangeRequests.FindAsync(request.Id, cancellationToken);
            if (entity == null)
                return Response.FailureResponse<ExpenditureChangeRequestDTO>("The specified request could not be located. Please verify the ID and try again.");
            var dto = new ExpenditureChangeRequestDTO
            {
                Id = entity.Id,
                CurrentExpenditure = entity.CurrentExpenditureBranchId.ToString(),
                NewExpenditure = entity.NewExpenditureBranchId.ToString(),
                Reason = entity.Reason
            };
            return new(data: dto);
        }
    }
} 