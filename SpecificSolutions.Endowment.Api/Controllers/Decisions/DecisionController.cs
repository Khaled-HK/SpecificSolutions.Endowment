using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Decisions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Decisions
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DecisionController : ApiController
    {
        private readonly IMediator _mediator;

        public DecisionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<EndowmentResponse>> CreateDecision([FromBody] CreateDecisionCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid request data.");
            }

            var response = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateDecision), response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EndowmentResponse>> UpdateDecision(Guid id, [FromBody] UpdateDecisionCommand command)
        {
            if (command == null || id != command.Id)
            {
                return BadRequest("Invalid request data.");
            }

            var response = await _mediator.Send(command);
            if (!response.IsSuccess)
            {
                return NotFound("Decision not found.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EndowmentResponse>> DeleteDecision(Guid id)
        {
            var command = new DeleteDecisionCommand(id);
            var response = await _mediator.Send(command);
            if (!response.IsSuccess)
            {
                return NotFound("Decision not found.");
            }

            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<EndowmentResponse<PagedList<FilterDecisionDTO>>>> FilterDecisions([FromQuery] FilterDecisionQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}