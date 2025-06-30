using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Queries.Filter;

namespace SpecificSolutions.Endowment.Api.Controllers.BuildingDetailRequests
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BuildingDetailRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BuildingDetailRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBuildingDetailRequestCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateBuildingDetailRequestCommand command)
        {
            if (id != command.Id) return BadRequest();
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteBuildingDetailRequestCommand { Id = id });
            return Ok(response);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] FilterBuildingDetailRequestQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}