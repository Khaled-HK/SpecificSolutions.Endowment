using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.ExpenditureChangeRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Queries.GetById
{
    public record GetByIdQuery(Guid Id) : IRequest<EndowmentResponse<ExpenditureChangeRequestDTO>>;
} 