using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MaintenanceRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MaintenanceRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateMaintenanceRequestCommand command)
        {
            return await _mediator.Send(command);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(Guid id)
        //{
        //    var maintenanceRequest = await _mediator.Send(new GetMaintenanceRequestByIdQuery { MaintenanceRequestID = id });
        //    if (maintenanceRequest == null) return NotFound();
        //    return Ok(maintenanceRequest);
        //}

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid Id, UpdateMaintenanceRequestCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteMaintenanceRequestCommand { MaintenanceRequestID = id });
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string searchTerm)
        {
            var maintenanceRequests = await _mediator.Send(new FilterMaintenanceRequestQuery { SearchTerm = searchTerm });
            return Ok(maintenanceRequests);
        }
    }
}