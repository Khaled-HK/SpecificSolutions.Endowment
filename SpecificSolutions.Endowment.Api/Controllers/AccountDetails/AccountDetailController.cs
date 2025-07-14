using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Queries.GetAccountDetail;
using SpecificSolutions.Endowment.Application.Handlers.AccountDetails.Queries.GetAccountDetails;
using SpecificSolutions.Endowment.Application.Models.DTOs.AccountDetails;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.AccountDetails
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountDetailController : ApiController
    {
        private readonly IMediator _mediator;

        public AccountDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateAccountDetailCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateAccountDetailCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteAccountDetailCommand(id), cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetAccountDetailById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetAccountDetailQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<FilterAccountDetailDTO>>> Filter([FromQuery] FilterAccountDetailQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetAccountDetails")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetAccountDetails([FromQuery] GetAccountDetailsQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}
