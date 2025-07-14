using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Queries.GetById;
using SpecificSolutions.Endowment.Application.Handlers.FacilityDetails.Queries.GetEntities;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FacilityDetailController : ApiController
    {
        private readonly IMediator _mediator;

        public FacilityDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateFacilityDetailCommand command, CancellationToken cancellationToken = default) =>
            await _mediator.Send(command, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetById(Guid id, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new GetFacilityDetailByIdQuery(id), cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateFacilityDetailCommand command, CancellationToken cancellationToken = default) =>
            await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new DeleteFacilityDetailCommand { Id = id }, cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse> Filter([FromQuery] string searchTerm, CancellationToken cancellationToken = default) =>
            await _mediator.Send(new FilterFacilityDetailQuery { SearchTerm = searchTerm }, cancellationToken);

        [HttpGet("entities")]
        public async Task<EndowmentResponse> GetEntities(CancellationToken cancellationToken = default) =>
            await _mediator.Send(new GetFacilityDetailEntitiesQuery(), cancellationToken);
    }
}