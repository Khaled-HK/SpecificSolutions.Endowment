using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Mosques;
using SpecificSolutions.Endowment.Application.Models.Global;
using System;
using System.Threading.Tasks;

namespace SpecificSolutions.Endowment.Management.Controllers.Mosques
{
    [ApiController]
    [Route("api/[controller]")]
    public class MosqueController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MosqueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMosqueCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateMosqueCommand command)
        {
            if (id != command.Id) return BadRequest();
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteMosqueCommand { Id = id });
            return Ok(response);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] FilterMosqueQuery queryParams)
        {
            var response = await _mediator.Send(queryParams);
            return Ok(response);
        }
    }
}