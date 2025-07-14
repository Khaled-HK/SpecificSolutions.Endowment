using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Banks.Commands.Delete
{
    public class DeleteBankHandler : ICommandHandler<DeleteBankCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBankHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteBankCommand request, CancellationToken cancellationToken)
        {
            var bank = await _unitOfWork.Banks.GetByIdAsync(request.Id, cancellationToken);
            if (bank == null)
                return Response.FailureResponse("Bank not found.");

            await _unitOfWork.Banks.DeleteAsync(request.Id);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}