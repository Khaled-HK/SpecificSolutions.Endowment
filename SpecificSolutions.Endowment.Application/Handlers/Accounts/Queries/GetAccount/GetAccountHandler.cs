using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Accounts;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Accounts.Queries.GetAccount
{
    public class GetAccountHandler : IRequestHandler<GetAccountQuery, EndowmentResponse<FilterAccountDTO>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<EndowmentResponse<FilterAccountDTO>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.Id);
            if (account == null)
            {
                return Response.FailureResponse<FilterAccountDTO>("The specified account could not be located. Please verify the account ID and try again.");
            }

            var accountDTO = new FilterAccountDTO
            {
                Id = account.Id,
                Name = account.Name,
                MotherName = account.MotherName,
                BirthDate = account.BirthDate,
                Gender = account.Gender,
                Barcode = account.Barcode,
                Status = account.Status,
                LockerFileNumber = account.LockerFileNumber,
                SocialStatus = account.SocialStatus,
                BookNumber = account.BookNumber,
                PaperNumber = account.PaperNumber,
                RegistrationNumber = account.RegistrationNumber,
                AccountNumber = account.AccountNumber,
                Type = account.Type,
                LookOver = account.LookOver,
                Note = account.Note,
                NID = account.NID,
                IsActive = account.IsActive,
                Balance = account.Balance
            };

            return new(data: accountDTO );
        }
    }
}