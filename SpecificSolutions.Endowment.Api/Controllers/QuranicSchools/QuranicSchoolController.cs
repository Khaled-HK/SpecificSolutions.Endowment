using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Queries.GetQuranicSchool;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Queries.GetQuranicSchools;
using SpecificSolutions.Endowment.Application.Models.DTOs.QuranicSchools;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.QuranicSchools
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class QuranicSchoolController : ApiController
    {
        private readonly IMediator _mediator;

        public QuranicSchoolController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateQuranicSchoolCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateQuranicSchoolCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteQuranicSchoolCommand { Id = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetQuranicSchoolById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetQuranicSchoolQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<QuranicSchoolDTO>>> Filter([FromQuery] FilterQuranicSchoolQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetQuranicSchools")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetQuranicSchools([FromQuery] GetQuranicSchoolsQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}
