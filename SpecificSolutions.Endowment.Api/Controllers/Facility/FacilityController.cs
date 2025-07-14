using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Queries.GetFacilities;
using SpecificSolutions.Endowment.Application.Handlers.Facilities.Queries.GetFacility;
using SpecificSolutions.Endowment.Application.Models.DTOs.Facilities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Facility
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FacilityController : ApiController
    {
        private readonly IMediator _mediator;

        public FacilityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateFacilityCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateFacilityCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteFacilityCommand { Id = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetFacilityById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetFacilityQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<FacilityDTO>>> Filter([FromQuery] FilterFacilityQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetFacilities")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetFacilities([FromQuery] GetFacilitiesQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}