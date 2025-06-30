using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class NeedsRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NeedsRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateNeedsRequestCommand command)
        {
            return await _mediator.Send(command);
        }


        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid Id, UpdateNeedsRequestCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteNeedsRequestCommand { NeedsRequestID = id });
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string searchTerm)
        {
            var needsRequests = await _mediator.Send(new FilterNeedsRequestQuery { SearchTerm = searchTerm });
            return Ok(needsRequests);
        }
    }
}