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
        public async Task<EndowmentResponse> CreateMosque([FromBody] CreateMosqueCommand command)
        {
            var response = await _mediator.Send(command);
            return response;
        }

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateMosqueCommand command)
        {
            //if (id != command.Id) return BadRequest();
            var response = await _mediator.Send(command);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id)
        {
            var response = await _mediator.Send(new DeleteMosqueCommand { Id = id });
            return response;
        }

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<MosqueDTO>>> Filter([FromQuery] FilterMosqueQuery queryParams)
        {
            var response = await _mediator.Send(queryParams);
            return response;
        }
    }
}