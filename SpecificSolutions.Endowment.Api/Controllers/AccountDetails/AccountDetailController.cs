using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Queries.Filter;

namespace SpecificSolutions.Endowment.Api.Controllers.AccountDetails
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountDetailController : ApiController
    {
        private readonly IMediator _mediator;

        public AccountDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateAccountDetail([FromBody] CreateAccountDetailCommand command)
        //{
        //    var response = await _mediator.Send(command);
        //    if (response.IsSuccess)
        //    {
        //        return CreatedAtAction(nameof(GetAccountDetailById), new { id = response.IsSuccess }, response);
        //    }
        //    return BadRequest(response);
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccountDetail(Guid id, [FromBody] UpdateAccountDetailCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }

            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(response);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetAccountDetailById(Guid id)
        //{
        //    var query = new GetAccountDetailQuery { Id = id };
        //    var response = await _mediator.Send(query);
        //    if (response.IsSuccess)
        //    {
        //        return Ok(response.Data);
        //    }
        //    return NotFound();
        //}

        [HttpGet]
        public async Task<IActionResult> FilterAccountDetails([FromQuery] FilterAccountDetailQuery query)
        {
            var response = await _mediator.Send(query);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountDetail(Guid id)
        {
            var command = new DeleteAccountDetailCommand(id);
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
