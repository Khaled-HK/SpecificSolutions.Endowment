using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Banks;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Banks.Queries.GetBank
{
    public class GetBankHandler : IRequestHandler<GetBankQuery, EndowmentResponse<BankDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBankHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<BankDTO>> Handle(GetBankQuery request, CancellationToken cancellationToken)
        {
            var bank = await _unitOfWork.Banks.GetByIdAsync(request.Id, cancellationToken);
            if (bank == null)
            {
                return Response.FailureResponse<BankDTO>("The specified Bank could not be located. Please verify the Bank ID and try again.");
            }

            var bankDTO = new BankDTO
            {
                Id = bank.Id,
                Name = bank.Name,
                Address = bank.Address,
                ContactNumber = bank.ContactNumber,
            };

            return new(data: bankDTO);
        }
    }
} 