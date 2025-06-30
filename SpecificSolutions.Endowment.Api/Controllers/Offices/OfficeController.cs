using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.GetOffices;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Offices
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfficeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OfficeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpPost]
        //public async Task<ActionResult<EndowmentResponse>> CreateOffice([FromBody] CreateOfficeCommand command)
        //{
        //    if (command == null)
        //    {
        //        return BadRequest("Invalid request data.");
        //    }

        //    var officeId = await _mediator.Send(command);
        //    return CreatedAtAction(nameof(GetOfficeById), new { id = officeId }, command);
        //}

        [HttpPut("{id}")]
        public async Task<ActionResult<EndowmentResponse>> UpdateOffice(Guid id, [FromBody] UpdateOfficeCommand command)
        {
            if (command == null || id != command.Id)
            {
                return BadRequest("Invalid request data.");
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EndowmentResponse>> DeleteOffice(Guid id)
        {
            var command = new DeleteOfficeCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<EndowmentResponse<FilterOfficeDTO>>> GetOfficeById(Guid id)
        //{
        //    var query = new GetOfficeQuery { Id = id };
        //    var response = await _mediator.Send(query);
        //    if (response.IsSuccess)
        //    {
        //        return Ok(response.Data);
        //    }
        //    return NotFound();
        //}

        [HttpGet("GetOffices")]
        public async Task<EndowmentResponse> FilterOffices([FromQuery] GetOfficesQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}