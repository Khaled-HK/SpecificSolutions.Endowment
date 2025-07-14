using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Queries.GetDemolitionRequest;
using SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Queries.GetDemolitionRequests;
using SpecificSolutions.Endowment.Application.Models.DTOs.DemolitionRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.DemolitionRequests
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DemolitionRequestController : ApiController
    {
        private readonly IMediator _mediator;

        public DemolitionRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateDemolitionRequestCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateDemolitionRequestCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteDemolitionRequestCommand { DemolitionRequestID = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetDemolitionRequestById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetDemolitionRequestQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<FilterDemolitionRequestDTO>>> Filter([FromQuery] FilterDemolitionRequestQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetDemolitionRequests")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetDemolitionRequests([FromQuery] GetDemolitionRequestsQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}