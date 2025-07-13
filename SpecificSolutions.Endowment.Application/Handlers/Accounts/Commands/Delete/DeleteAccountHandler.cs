using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Delete
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountCommand, EndowmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAccountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(command.Id, cancellationToken);
            if (account == null)
            {
                return Response.FailureResponse("Id", "Account not found.");
            }

            await _unitOfWork.Accounts.RemoveAsync(account);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}