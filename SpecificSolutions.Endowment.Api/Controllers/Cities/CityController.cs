using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Cities.Queries.Filter;

namespace SpecificSolutions.Endowment.Api.Controllers.Cities
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCityCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCityCommand command)
        {
            if (id != command.Id) return BadRequest();
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteCityCommand { Id = id });
            return Ok(response);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] FilterCityQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}