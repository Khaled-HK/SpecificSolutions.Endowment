using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.BuildingDetails.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.BuildingDetails;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.BuildingDetails
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingDetailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BuildingDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateBuildingDetailCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return response;
        }

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateBuildingDetailCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new DeleteBuildingDetailCommand { Id = id }, cancellationToken);
            return response;
        }

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<BuildingDetailDTO>>> Filter([FromQuery] FilterBuildingDetailQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}