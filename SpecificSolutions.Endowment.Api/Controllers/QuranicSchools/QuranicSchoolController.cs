using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Queries.Filter;

namespace SpecificSolutions.Endowment.Api.Controllers.QuranicSchools
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class QuranicSchoolController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuranicSchoolController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create(CreateQuranicSchoolCommand command)
        {
            var quranicSchool = await _mediator.Send(command);
            return Ok(quranicSchool);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateQuranicSchoolCommand command)
        {
            if (id != command.Id) return BadRequest();
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteQuranicSchoolCommand { Id = id });
            return Ok(response);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] FilterQuranicSchoolQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
