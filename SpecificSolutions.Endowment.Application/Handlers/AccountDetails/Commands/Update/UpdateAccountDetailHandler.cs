using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Commands.Update
{
    public class UpdateAccountDetailHandler : ICommandHandler<UpdateAccountDetailCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAccountDetailHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateAccountDetailCommand command, CancellationToken cancellationToken)
        {
            var accountDetail = await _unitOfWork.AccountDetails.GetByIdAsync(command.Id);
            if (accountDetail == null)
            {
                return Response.FailureResponse("Id", "AccountDetail not found.");
            }
            accountDetail.Update(command);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}
