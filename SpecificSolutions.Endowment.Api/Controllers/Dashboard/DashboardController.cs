using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.Dashboard.Queries.Filter;
using SpecificSolutions.Endowment.Application.Models.DTOs.Dashboard;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.Dashboard
{
    public class DashboardController : ApiController
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("summary")]
        public async Task<EndowmentResponse<DashboardSummaryDTO>> Summary([FromQuery] FilterDashboardQuery query, CancellationToken cancellationToken)
            => await _mediator.Send(query, cancellationToken);
    }
}


