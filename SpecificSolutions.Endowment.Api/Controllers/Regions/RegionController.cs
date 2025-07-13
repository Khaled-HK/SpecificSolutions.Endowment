using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.GetRegion;
using SpecificSolutions.Endowment.Application.Handlers.Regions.Queries.GetRegions;
using SpecificSolutions.Endowment.Application.Models.DTOs.Regions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Regions
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateRegionCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateRegionCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteRegionCommand { Id = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetRegionById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetRegionQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<FilterRegionDTO>>> Filter([FromQuery] FilterRegionQuery query, CancellationToken cancellationToken)
             => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetRegions")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> FilterRegions([FromQuery] GetRegionsQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}