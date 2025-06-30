using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Queries.Filter;

namespace SpecificSolutions.Endowment.Api.Controllers.Buildings
{
    [ApiController]
    [Route("api/buildings")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BuildingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BuildingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create(CreateBuildingCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateBuildingCommand command)
        {
            if (id != command.Id) return BadRequest();
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteBuildingCommand { Id = id });
            return Ok(response);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] FilterBuildingQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}