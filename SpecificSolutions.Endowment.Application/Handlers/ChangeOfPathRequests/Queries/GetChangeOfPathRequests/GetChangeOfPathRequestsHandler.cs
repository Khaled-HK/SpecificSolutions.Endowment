using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Queries.GetChangeOfPathRequests
{
    public class GetChangeOfPathRequestsHandler : IRequestHandler<GetChangeOfPathRequestsQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetChangeOfPathRequestsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetChangeOfPathRequestsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.ChangeOfPathRequests.GetAllAsync(cancellationToken);
            var result = entities.Select(e => new KeyValuPair
            {
                Key = e.Id,
                Value = e.CurrentType + " -> " + e.NewType
            });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
} 