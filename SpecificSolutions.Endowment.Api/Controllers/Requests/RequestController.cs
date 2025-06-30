using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Requests;

[ApiExplorerSettings(IgnoreApi = true)]
public class RequestController : ApiController
{
    private readonly IMediator _mediator;

    public RequestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<EndowmentResponse>> CreateRequest([FromBody] CreateRequestCommand command)
    {
        if (command == null)
        {
            return BadRequest("Invalid request data.");
        }

        var requestId = await _mediator.Send(command);

        return CreatedAtAction(nameof(CreateRequest), new { id = requestId }, command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EndowmentResponse>> UpdateRequest(Guid id, [FromBody] UpdateRequestCommand command)
    {
        if (command == null || id != command.Id)
        {
            return BadRequest("Invalid request data.");
        }

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return NotFound("Request not found.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<EndowmentResponse>> DeleteRequest(Guid id)
    {
        var command = new DeleteRequestCommand(id);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return NotFound("Request not found.");
        }

        return NoContent();
    }

    [HttpGet("filter")]
    public async Task<ActionResult<EndowmentResponse<PagedList<FilterRequestDTO>>>> FilterRequests([FromQuery] FilterRequestQuery query)
    {
        var requests = await _mediator.Send(query);

        return requests;
    }
}