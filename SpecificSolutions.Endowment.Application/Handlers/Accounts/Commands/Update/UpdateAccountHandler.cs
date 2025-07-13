using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Update
{
    public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, EndowmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAccountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateAccountCommand command, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(command.Id, cancellationToken);
            if (account == null)
            {
                return Response.FailureResponse("Id", "Account not found.");
            }

            account.Update(command);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}