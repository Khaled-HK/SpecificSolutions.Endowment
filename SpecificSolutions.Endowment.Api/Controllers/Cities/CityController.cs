using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Queries.GetCities;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Queries.GetCity;
using SpecificSolutions.Endowment.Application.Models.DTOs.Cities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Cities
{
    public class CityController : ApiController
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateCityCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateCityCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteCityCommand { Id = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetCityById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetCityQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<CityDTO>>> Filter([FromQuery] FilterCityQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetCities")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetCities([FromQuery] GetCitiesQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}