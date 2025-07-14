using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.GetMosque;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.GetMosques;
using SpecificSolutions.Endowment.Application.Models.DTOs.Mosques;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Mosques
{
    public class MosqueController : ApiController
    {
        private readonly IMediator _mediator;

        public MosqueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateMosqueCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateMosqueCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteMosqueCommand { Id = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetMosqueById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetMosqueQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<MosqueDTO>>> Filter([FromQuery] FilterMosqueQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetMosques")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetMosques([FromQuery] GetMosquesQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}