using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Branchs;

namespace SpecificSolutions.Endowment.Application.Handlers.Branches.Commands.Create
{
    public class CreateBranchHandler : ICommandHandler<CreateBranchCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBranchHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            var branch = Branch.Create(request.Name, request.Address, request.ContactNumber, request.Id);

            await _unitOfWork.Branches.AddAsync(branch);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}