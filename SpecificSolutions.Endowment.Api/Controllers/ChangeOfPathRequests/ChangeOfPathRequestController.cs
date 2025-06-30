using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ChangeOfPathRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChangeOfPathRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateChangeOfPathRequestCommand command)
        {
            return await _mediator.Send(command);
            //return CreatedAtAction(nameof(GetById), new { id = changeOfPathRequestId }, command);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(Guid id)
        //{
        //    var changeOfPathRequest = await _mediator.Send(new GetChangeOfPathRequestByIdQuery { ChangeOfPathRequestID = id });
        //    if (changeOfPathRequest == null) return NotFound();
        //    return Ok(changeOfPathRequest);
        //}

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid Id, UpdateChangeOfPathRequestCommand command)
        {

            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteChangeOfPathRequestCommand { ChangeOfPathRequestID = id });
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string searchTerm)
        {
            var changeOfPathRequests = await _mediator.Send(new FilterChangeOfPathRequestQuery { SearchTerm = searchTerm });
            return Ok(changeOfPathRequests);
        }
    }
}