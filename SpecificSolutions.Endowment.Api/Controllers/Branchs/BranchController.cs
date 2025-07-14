using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Branches.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Branches.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Branches.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Branches.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Branches.Queries.GetBranch;
using SpecificSolutions.Endowment.Application.Handlers.Branches.Queries.GetBranches;
using SpecificSolutions.Endowment.Application.Models.DTOs.Branchs;
using SpecificSolutions.Endowment.Application.Models.Global;
namespace SpecificSolutions.Endowment.Api.Controllers.Branchs
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BranchController : ApiController
    {
        private readonly IMediator _mediator;

        public BranchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateBranchCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateBranchCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteBranchCommand { Id = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetBranchById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetBranchQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<BranchDTO>>> Filter([FromQuery] FilterBranchQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetBranches")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetBranches([FromQuery] GetBranchesQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}