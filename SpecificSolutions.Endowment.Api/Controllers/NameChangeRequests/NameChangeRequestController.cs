using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.NameChangeRequests
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class NameChangeRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NameChangeRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateNameChangeRequestCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateNameChangeRequestCommand command)
        {
            if (id != command.Id) BadRequest();
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteNameChangeRequestCommand { Id = id });
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string searchTerm)
        {
            var maintenanceRequests = await _mediator.Send(new FilterNameChangeRequestQuery { SearchTerm = searchTerm });
            return Ok(maintenanceRequests);
        }
    }
}
