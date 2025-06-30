using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.DTOs.Requests;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Requests.Queries.Filter
{
    public class FilterRequestHandler : IRequestHandler<FilterRequestQuery, EndowmentResponse<PagedList<FilterRequestDTO>>>
    {
        private readonly IRequestRepository _requestRepository;

        public FilterRequestHandler(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<EndowmentResponse<PagedList<FilterRequestDTO>>> Handle(FilterRequestQuery query, CancellationToken cancellationToken)
        {
            var requests = await _requestRepository.GetByFilterAsync(query, cancellationToken);

            if (!requests.Items.Any())
            {
                return new EndowmentResponse<PagedList<FilterRequestDTO>>(PagedList<FilterRequestDTO>.Empty() );

            }

            return new EndowmentResponse<PagedList<FilterRequestDTO>>(requests );
        }
    }
}