using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Mosques;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Mosques
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
        public async Task<EndowmentResponse> CreateMosque([FromBody] CreateMosqueCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateMosqueCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteMosqueCommand { Id = id }, cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<MosqueDTO>>> Filter([FromQuery] FilterMosqueQuery queryParams, CancellationToken cancellationToken)
            => await _mediator.Send(queryParams, cancellationToken);
    }
}