using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Queries.GetById;
using SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Queries.GetEntities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    public class NeedsRequestController : ApiController
    {
        private readonly IMediator _mediator;

        public NeedsRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateNeedsRequestCommand command, CancellationToken cancellationToken = default) =>
            await _mediator.Send(command, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetById(Guid id, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new GetNeedsRequestByIdQuery(id), cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateNeedsRequestCommand command, CancellationToken cancellationToken = default) =>
            await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new DeleteNeedsRequestCommand { NeedsRequestID = id }, cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse> Filter([FromQuery] string searchTerm, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new FilterNeedsRequestQuery { SearchTerm = searchTerm }, cancellationToken);

        [HttpGet("needs-requests")]
        public async Task<EndowmentResponse> GetNeedsRequests(CancellationToken cancellationToken = default) =>
            await _mediator.Send(new GetNeedsRequestEntitiesQuery(), cancellationToken);
    }
}