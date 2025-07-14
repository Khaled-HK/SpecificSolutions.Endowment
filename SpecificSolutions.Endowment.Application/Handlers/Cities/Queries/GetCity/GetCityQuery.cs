using MediatR;
using SpecificSolutions.Endowment.Application.Models.DTOs.Cities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Queries.GetCity
{
    public record GetCityQuery(Guid Id) : IRequest<EndowmentResponse<CityDTO>>;
} 