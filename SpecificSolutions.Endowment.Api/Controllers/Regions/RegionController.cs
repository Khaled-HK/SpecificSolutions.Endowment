using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.GetRegions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Regions
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRegionCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateRegionCommand command)
        {
            if (id != command.Id) return BadRequest();
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteRegionCommand { Id = id });
            return Ok(response);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] FilterRegionQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        // get regions key value pairs for dropdown by filter by name   
        [HttpGet("dropdown")]
        public async Task<IActionResult> GetRegionsDropdown([FromQuery] FilterRegionQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("GetRegions")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> FilterRegions([FromQuery] GetRegionsQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}