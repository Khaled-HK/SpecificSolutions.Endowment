using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Queries.GetBuildingDetail;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Queries.GetBuildingDetails;
using SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetails;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.BuildingDetails
{
    public class BuildingDetailController : ApiController
    {
        private readonly IMediator _mediator;

        public BuildingDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateBuildingDetailCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateBuildingDetailCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteBuildingDetailCommand { Id = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetBuildingDetailById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetBuildingDetailQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<BuildingDetailDTO>>> Filter([FromQuery] FilterBuildingDetailQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetBuildingDetails")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetBuildingDetails([FromQuery] GetBuildingDetailsQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}