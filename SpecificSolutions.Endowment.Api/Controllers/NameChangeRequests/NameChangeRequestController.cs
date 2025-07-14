using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Queries.GetById;
using SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Queries.GetEntities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class NameChangeRequestController : ApiController
    {
        private readonly IMediator _mediator;

        public NameChangeRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateNameChangeRequestCommand command, CancellationToken cancellationToken = default) =>
            await _mediator.Send(command, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetById(Guid id, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new GetNameChangeRequestByIdQuery(id), cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateNameChangeRequestCommand command, CancellationToken cancellationToken = default) =>
            await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new DeleteNameChangeRequestCommand { Id = id }, cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse> Filter([FromQuery] string searchTerm, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new FilterNameChangeRequestQuery { SearchTerm = searchTerm }, cancellationToken);

        [HttpGet("name-change-requests")]
        public async Task<EndowmentResponse> GetNameChangeRequests(CancellationToken cancellationToken = default) =>
            await _mediator.Send(new GetNameChangeRequestEntitiesQuery(), cancellationToken);
    }
}
