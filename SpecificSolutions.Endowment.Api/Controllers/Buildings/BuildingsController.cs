using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Queries.GetBuilding;
using SpecificSolutions.Endowment.Application.Handlers.Buildings.Queries.GetBuildings;
using SpecificSolutions.Endowment.Application.Models.DTOs.Buildings;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Buildings
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BuildingsController : ApiController
    {
        private readonly IMediator _mediator;

        public BuildingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateBuildingCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateBuildingCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteBuildingCommand { Id = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetBuildingById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetBuildingQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<BuildingDTO>>> Filter([FromQuery] FilterBuildingQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetBuildings")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetBuildings([FromQuery] GetBuildingsQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}