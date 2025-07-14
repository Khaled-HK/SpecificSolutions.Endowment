using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Branches.Commands.Update
{
    public class UpdateBranchHandler : ICommandHandler<UpdateBranchCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBranchHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(request.Id, cancellationToken);
            if (branch == null)
                return Response.FailureResponse("Branch not found.");

            await _unitOfWork.Branches.UpdateAsync(branch);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}