using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Queries.GetAccount;
using SpecificSolutions.Endowment.Application.Handlers.Accounts.Queries.GetAccounts;
using SpecificSolutions.Endowment.Application.Models.DTOs.Accounts;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Accounts
{
    public class AccountController : ApiController
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateAccountCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateAccountCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteAccountCommand(id), cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetAccountById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetAccountQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<FilterAccountDTO>>> Filter([FromQuery] FilterAccountQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetAccounts")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetAccounts([FromQuery] GetAccountsQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}