using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.ChangeOfPathRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Queries.GetChangeOfPathRequest
{
    public record GetChangeOfPathRequestQuery(Guid Id) : IRequest<EndowmentResponse<ChangeOfPathRequestDTO>>;
} 