using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Banks;

namespace SpecificSolutions.Endowment.Application.Handlers.Banks.Commands.Create
{
    public class CreateBankHandler : ICommandHandler<CreateBankCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBankHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateBankCommand request, CancellationToken cancellationToken)
        {
            var bank = Bank.Create(request.Name, request.Address, request.ContactNumber);

            await _unitOfWork.Banks.AddAsync(bank, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}