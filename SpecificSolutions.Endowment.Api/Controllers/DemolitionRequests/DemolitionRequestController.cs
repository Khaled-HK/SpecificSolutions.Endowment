using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DemolitionRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DemolitionRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateDemolitionRequestCommand command)
        {
            return await _mediator.Send(command);
        }

        //[HttpGet("{id}")]
        //public async Task<EndowmentResponse> GetById(Guid id)
        //{
        //    var DemolitionRequest = await _mediator.Send(new GetDemolitionRequestByIdQuery { DemolitionRequestID = id });
        //    if (DemolitionRequest == null) return NotFound();
        //    return Ok(DemolitionRequest);
        //}

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid Id, UpdateDemolitionRequestCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteDemolitionRequestCommand { DemolitionRequestID = id });
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string searchTerm)
        {
            var DemolitionRequests = await _mediator.Send(new FilterDemolitionRequestQuery { SearchTerm = searchTerm });
            return Ok(DemolitionRequests);
        }
    }
}