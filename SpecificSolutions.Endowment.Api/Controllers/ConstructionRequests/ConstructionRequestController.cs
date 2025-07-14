using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Queries.GetConstructionRequest;
using SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Queries.GetConstructionRequests;
using SpecificSolutions.Endowment.Application.Models.DTOs.ConstructionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.ConstructionRequests
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ConstructionRequestController : ApiController
    {
        private readonly IMediator _mediator;

        public ConstructionRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateConstructionRequestCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateConstructionRequestCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteConstructionRequestCommand { ConstructionRequestID = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetConstructionRequestById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetConstructionRequestQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<ConstructionRequestDTO>>> Filter([FromQuery] FilterConstructionRequestQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetConstructionRequests")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetConstructionRequests([FromQuery] GetConstructionRequestsQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}