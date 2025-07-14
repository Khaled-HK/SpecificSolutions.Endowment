using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Branchs;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Branches.Queries.GetBranch
{
    public class GetBranchHandler : IRequestHandler<GetBranchQuery, EndowmentResponse<BranchDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBranchHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<BranchDTO>> Handle(GetBranchQuery request, CancellationToken cancellationToken)
        {
            var branch = await _unitOfWork.Branches.GetByIdAsync(request.Id, cancellationToken);
            if (branch == null)
            {
                return Response.FailureResponse<BranchDTO>("The specified branch could not be located. Please verify the branch ID and try again.");
            }

            var branchDTO = new BranchDTO
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                ContactNumber = branch.ContactNumber,
                BankId = branch.BankId
            };

            return new(data: branchDTO);
        }
    }
} 