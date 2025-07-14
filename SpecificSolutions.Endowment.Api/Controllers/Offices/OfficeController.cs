using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.GetOffice;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Queries.GetOffices;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Offices
{
    public class OfficeController : ApiController
    {
        private readonly IMediator _mediator;

        public OfficeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateOfficeCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateOfficeCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteOfficeCommand { Id = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetOfficeById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetOfficeQuery(id), cancellationToken);

        [HttpGet("GetOffices")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetOffices([FromQuery] GetOfficesQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}