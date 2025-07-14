using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.Branchs;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Branches.Queries.GetBranch
{
    public record GetBranchQuery(Guid Id) : IRequest<EndowmentResponse<BranchDTO>>;
} 