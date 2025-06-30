using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Queries.Filter;

namespace SpecificSolutions.Endowment.Api.Controllers.FacilityDetails
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FacilityDetailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FacilityDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFacilityDetailCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateFacilityDetailCommand command)
        {
            if (id != command.Id) return BadRequest();
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteFacilityDetailCommand { Id = id });
            return Ok(response);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] FilterFacilityDetailQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}