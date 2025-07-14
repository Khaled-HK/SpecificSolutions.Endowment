using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.Decisions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.GetById
{
    public record GetDecisionByIdQuery(Guid Id) : IRequest<EndowmentResponse<FilterDecisionDTO>>;
} 