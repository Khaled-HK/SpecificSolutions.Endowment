using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.AccountDetails;

namespace SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Commands.Create
{
    public class CreateAccountDetailHandler : ICommandHandler<CreateAccountDetailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAccountDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateAccountDetailCommand command, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(command.Id, cancellationToken);
            if (account == null)
            {
                return Response.FailureResponse("The specified account could not be located. Please verify the account ID and try again.");
            }

            var accountDetail = new AccountDetail(command);

            await _unitOfWork.AccountDetails.AddAsync(accountDetail, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}
