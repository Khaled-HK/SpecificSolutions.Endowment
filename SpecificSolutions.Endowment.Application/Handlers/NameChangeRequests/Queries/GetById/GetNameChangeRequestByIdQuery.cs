using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.NameChangeRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Queries.GetById
{
    public record GetNameChangeRequestByIdQuery(Guid Id) : IRequest<EndowmentResponse<NameChangeRequestDTO>>;
} 