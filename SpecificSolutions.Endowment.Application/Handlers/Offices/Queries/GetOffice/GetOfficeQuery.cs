using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.Offices;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.GetOffice
{
    public record GetOfficeQuery(Guid Id) : IRequest<EndowmentResponse<OfficeDTO>>;
} 