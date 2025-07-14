using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.AccountDetails;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Queries.GetAccountDetail
{
    public class GetAccountDetailHandler : IRequestHandler<GetAccountDetailQuery, EndowmentResponse<AccountDetailDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAccountDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<AccountDetailDTO>> Handle(GetAccountDetailQuery request, CancellationToken cancellationToken)
        {
            var accountDetail = await _unitOfWork.AccountDetails.GetByIdAsync(request.Id, cancellationToken);
            if (accountDetail == null)
            {
                return Response.FailureResponse<AccountDetailDTO>("The specified account detail could not be located. Please verify the ID and try again.");
            }

            var accountDetailDTO = new AccountDetailDTO
            {
                Id = accountDetail.Id,
                Debtor = accountDetail.Debtor,
                Creditor = accountDetail.Creditor,
                Note = accountDetail.Note,
                OperationType = accountDetail.OperationType,
                OperationNumber = accountDetail.OperationNumber,
                Balance = accountDetail.Balance,
                AccountId = accountDetail.AccountId,
                CreatedDate = accountDetail.CreatedDate
            };

            return new(data: accountDetailDTO);
        }
    }
} 