using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Queries.GetAccount;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Accounts
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : ApiController
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> CreateAccount([FromBody] CreateAccountCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAccountCommand command)
        {
            if (command == null || id != command.Id)
            {
                return BadRequest("Invalid request data.");
            }

            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(Guid id)
        {
            var query = new GetAccountQuery(id);
            var response = await _mediator.Send(query);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> FilterAccounts([FromQuery] FilterAccountQuery query)
        {
            var response = await _mediator.Send(query);

            if (response.IsSuccess)
                return Ok(response.Data);

            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var command = new DeleteAccountCommand(id);
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}