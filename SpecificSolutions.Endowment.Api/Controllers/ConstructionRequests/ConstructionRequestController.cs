using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ConstructionRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConstructionRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateConstructionRequestCommand command)
        {
            return await _mediator.Send(command);
        }


        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid Id, UpdateConstructionRequestCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteConstructionRequestCommand { ConstructionRequestID = id });
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string searchTerm)
        {
            var constructionRequests = await _mediator.Send(new FilterConstructionRequestQuery { SearchTerm = searchTerm });
            return Ok(constructionRequests);
        }
    }
}