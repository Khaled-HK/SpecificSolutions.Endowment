using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Queries.GetMaintenanceRequest;
using SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Queries.GetMaintenanceRequests;
using SpecificSolutions.Endowment.Application.Models.DTOs.MaintenanceRequests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.MaintenanceRequests
{
    public class MaintenanceRequestController : ApiController
    {
        private readonly IMediator _mediator;

        public MaintenanceRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateMaintenanceRequestCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateMaintenanceRequestCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteMaintenanceRequestCommand { MaintenanceRequestID = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetMaintenanceRequestById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetMaintenanceRequestQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<MaintenanceRequestDTO>>> Filter([FromQuery] FilterMaintenanceRequestQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetMaintenanceRequests")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetMaintenanceRequests([FromQuery] GetMaintenanceRequestsQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}