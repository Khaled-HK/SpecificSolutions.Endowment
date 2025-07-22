using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.GetById;
using SpecificSolutions.Endowment.Application.Handlers.Decisions.Queries.GetDecisions;
using SpecificSolutions.Endowment.Application.Models.DTOs.Decisions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Decisions
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DecisionController : ApiController
    {
        private readonly IMediator _mediator;

        public DecisionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateDecisionCommand command, CancellationToken cancellationToken = default) =>
            await _mediator.Send(command, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetById(Guid id, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new GetDecisionByIdQuery(id), cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateDecisionCommand command, CancellationToken cancellationToken = default) =>
            await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new DeleteDecisionCommand(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<FilterDecisionDTO>>> Filter([FromQuery] FilterDecisionQuery query, CancellationToken cancellationToken = default) =>
            await _mediator.Send(query, cancellationToken);

        [HttpGet("GetDecisions")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetDecisions([FromQuery] GetDecisionsQuery query, CancellationToken cancellationToken = default) =>
            await _mediator.Send(query, cancellationToken);
    }
}