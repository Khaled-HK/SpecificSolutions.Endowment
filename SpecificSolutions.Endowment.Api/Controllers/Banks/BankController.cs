using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Banks.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Banks.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Banks.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Banks.Queries.Filter;

namespace SpecificSolutions.Endowment.Api.Controllers.Banks
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BankController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BankController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBankCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateBankCommand command)
        {
            if (id != command.Id) return BadRequest();
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteBankCommand { Id = id });
            return Ok(response);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] FilterBankQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}