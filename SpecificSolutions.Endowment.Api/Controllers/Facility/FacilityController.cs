using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Queries.Filter;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FacilityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FacilityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFacilityCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateFacilityCommand command)
        {
            if (id != command.Id) return BadRequest();
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteFacilityCommand { Id = id });
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string searchTerm)
        {
            var facilities = await _mediator.Send(new FilterFacilityQuery { SearchTerm = searchTerm });
            return Ok(facilities);
        }
    }
}