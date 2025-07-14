using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Queries.GetById;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetailRequests.Queries.GetEntities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BuildingDetailRequestController : ApiController
    {
        private readonly IMediator _mediator;

        public BuildingDetailRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateBuildingDetailRequestCommand command, CancellationToken cancellationToken = default) =>
            await _mediator.Send(command, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetById(Guid id, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new GetBuildingDetailRequestByIdQuery(id), cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateBuildingDetailRequestCommand command, CancellationToken cancellationToken = default) =>
            await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new DeleteBuildingDetailRequestCommand { Id = id }, cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse> Filter([FromQuery] string searchTerm, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new FilterBuildingDetailRequestQuery { SearchTerm = searchTerm }, cancellationToken);

        [HttpGet("building-detail-requests")]
        public async Task<EndowmentResponse> GetBuildingDetailRequests(CancellationToken cancellationToken = default) =>
            await _mediator.Send(new GetBuildingDetailRequestEntitiesQuery(), cancellationToken);
    }
}