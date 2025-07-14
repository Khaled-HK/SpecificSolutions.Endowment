using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Requests.Queries.GetById
{
    public record GetRequestByIdQuery(Guid Id) : IRequest<EndowmentResponse<FilterRequestDTO>>;
} 