using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Queries.GetById;
using SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Queries.GetExpenditureChangeRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ExpenditureChangeRequestController : ApiController
    {
        private readonly IMediator _mediator;

        public ExpenditureChangeRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateExpenditureChangeRequestCommand command, CancellationToken cancellationToken = default) =>
            await _mediator.Send(command, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetById(Guid id, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new GetByIdQuery(id), cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateExpenditureChangeRequestCommand command, CancellationToken cancellationToken = default) =>
            await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new DeleteExpenditureChangeRequestCommand { EndowmentExpenditureChangeRequestID = id }, cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse> Filter([FromQuery] string searchTerm, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new FilterExpenditureChangeRequestQuery { SearchTerm = searchTerm }, cancellationToken);

        [HttpGet("GetExpenditureChangeRequests")]
        public async Task<EndowmentResponse> GetExpenditureChangeRequests(CancellationToken cancellationToken = default) =>
            await _mediator.Send(new GetExpenditureChangeRequestsQuery(), cancellationToken);
    }
}