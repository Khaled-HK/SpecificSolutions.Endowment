using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Banks.Commands.Create;
using SpecificSolutions.Endowment.Application.Handlers.Banks.Commands.Delete;
using SpecificSolutions.Endowment.Application.Handlers.Banks.Commands.Update;
using SpecificSolutions.Endowment.Application.Handlers.Banks.Queries.Filter;
using SpecificSolutions.Endowment.Application.Handlers.Banks.Queries.GetBank;
using SpecificSolutions.Endowment.Application.Handlers.Banks.Queries.GetBanks;
using SpecificSolutions.Endowment.Application.Models.DTOs.Banks;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Banks
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BankController : ApiController
    {
        private readonly IMediator _mediator;

        public BankController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<EndowmentResponse> Create(CreateBankCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpPut("{id}")]
        public async Task<EndowmentResponse> Update(Guid id, UpdateBankCommand command, CancellationToken cancellationToken)
            => await _mediator.Send(command, cancellationToken);

        [HttpDelete("{id}")]
        public async Task<EndowmentResponse> Delete(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteBankCommand { Id = id }, cancellationToken);

        [HttpGet("{id}")]
        public async Task<EndowmentResponse> GetBankById(Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new GetBankQuery(id), cancellationToken);

        [HttpGet("filter")]
        public async Task<EndowmentResponse<PagedList<BankDTO>>> Filter([FromQuery] FilterBankQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);

        [HttpGet("GetBanks")]
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> GetBanks([FromQuery] GetBanksQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}