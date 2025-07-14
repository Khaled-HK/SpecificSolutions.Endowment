using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Queries.GetChangeOfPathRequest;
using SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Queries.GetChangeOfPathRequests;
using SpecificSolutions.Endowment.Application.Models.DTOs.ChangeOfPathRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.ChangeOfPathRequests
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ChangeOfPathRequestController : ApiController
    {
        private readonly IMediator _mediator;

        public ChangeOfPathRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateChangeOfPathRequestCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateChangeOfPathRequestCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteChangeOfPathRequestCommand { ChangeOfPathRequestID = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetChangeOfPathRequestById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetChangeOfPathRequestQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<ChangeOfPathRequestDTO>>> Filter([FromQuery] FilterChangeOfPathRequestQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetChangeOfPathRequests")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetChangeOfPathRequests([FromQuery] GetChangeOfPathRequestsQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}