using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ExpenditureChangeRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpenditureChangeRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateExpenditureChangeRequestCommand command)
        {
            return await _mediator.Send(command);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(Guid id)
        //{
        //    var endowmentExpenditureChangeRequest = await _mediator.Send(new GetEndowmentExpenditureChangeRequestByIdQuery { EndowmentExpenditureChangeRequestID = id });
        //    if (endowmentExpenditureChangeRequest == null) return NotFound();
        //    return Ok(endowmentExpenditureChangeRequest);
        //}

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid Id, UpdateExpenditureChangeRequestCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteExpenditureChangeRequestCommand { EndowmentExpenditureChangeRequestID = id });
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string searchTerm)
        {
            var endowmentExpenditureChangeRequests = await _mediator.Send(new FilterExpenditureChangeRequestQuery { SearchTerm = searchTerm });
            return Ok(endowmentExpenditureChangeRequests);
        }
    }
}