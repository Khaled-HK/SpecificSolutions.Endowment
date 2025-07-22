using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Queries.GetById;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Queries.GetEntities;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Application.Models.DTOs.FacilityDetails;

namespace SpecificSolutions.Endowment.Api.Controllers.FacilityDetails
{
    public class FacilityDetailController : ApiController
    {
        private readonly IMediator _mediator;

        public FacilityDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateFacilityDetailCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateFacilityDetailCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteFacilityDetailCommand { Id = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetFacilityDetailById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetFacilityDetailByIdQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<FacilityDetailDTO>>> Filter([FromQuery] FilterFacilityDetailQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetFacilityDetails")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetFacilityDetails([FromQuery] GetFacilityDetailEntitiesQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}