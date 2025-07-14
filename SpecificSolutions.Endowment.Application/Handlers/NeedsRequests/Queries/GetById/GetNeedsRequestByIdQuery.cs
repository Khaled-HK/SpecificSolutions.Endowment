using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.NeedsRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Queries.GetById
{
    public record GetNeedsRequestByIdQuery(Guid NeedsRequestID) : IRequest<EndowmentResponse<FilterNeedsRequestDTO>>;
} 