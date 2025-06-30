using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Banks.Commands.Update
{
    public class UpdateBankHandler : ICommandHandler<UpdateBankCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBankHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateBankCommand request, CancellationToken cancellationToken)
        {
            var bank = await _unitOfWork.Banks.GetByIdAsync(request.Id);
            if (bank == null)
                return Response.FailureResponse("Bank not found.");


            await _unitOfWork.Banks.UpdateAsync(bank);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}