using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Branches.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Branches.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Branches.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Branches.Queries.Filter;

namespace SpecificSolutions.Endowment.Api.Controllers.Branchs
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BranchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BranchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBranchCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateBranchCommand command)
        {
            if (id != command.Id) return BadRequest();
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteBranchCommand { Id = id });
            return Ok(response);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] FilterBranchQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}