using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Commands.Delete
{
    public class DeleteAccountDetailHandler : ICommandHandler<DeleteAccountDetailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAccountDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteAccountDetailCommand command, CancellationToken cancellationToken)
        {
            var accountDetail = await _unitOfWork.AccountDetailRepository.GetByIdAsync(command.Id);
            if (accountDetail == null)
            {
                return Response.FailureResponse("Id", "AccountDetail not found.");
            }

            await _unitOfWork.AccountDetailRepository.RemoveAsync(accountDetail);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}