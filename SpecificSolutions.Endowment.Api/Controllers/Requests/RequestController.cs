using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Queries.GetById;
using SpecificSolutions.Endowment.Application.Handlers.Requests.Queries.GetEntities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Requests;

[ApiExplorerSettings(IgnoreApi = true)]
public class RequestController : ApiController
{
    private readonly IMediator _mediator;

    public RequestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<EndowmentResponse> Create(CreateRequestCommand command, CancellationToken cancellationToken = default) =>
        await _mediator.Send(command, cancellationToken);

    [HttpGet("{id}")]
    public async Task<EndowmentResponse> GetById(Guid id, CancellationToken cancellationToken = default) =>
        await _mediator.Send(new GetRequestByIdQuery(id), cancellationToken);

    [HttpPut("{id}")]
    public async Task<EndowmentResponse> Update(Guid id, UpdateRequestCommand command, CancellationToken cancellationToken = default) =>
        await _mediator.Send(command, cancellationToken);

    [HttpDelete("{id}")]
    public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken = default) =>
        await _mediator.Send(new DeleteRequestCommand(id), cancellationToken);

    [HttpGet("filter")]
    public async Task<EndowmentResponse> Filter([FromQuery] string searchTerm, CancellationToken cancellationToken = default) =>
        await _mediator.Send(new FilterRequestQuery { Title = searchTerm }, cancellationToken);

    [HttpGet("requests")]
    public async Task<EndowmentResponse> GetRequests(CancellationToken cancellationToken = default) =>
        await _mediator.Send(new GetRequestEntitiesQuery(), cancellationToken);
}