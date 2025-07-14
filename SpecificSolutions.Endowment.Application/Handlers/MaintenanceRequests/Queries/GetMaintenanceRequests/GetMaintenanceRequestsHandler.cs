using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Queries.GetMaintenanceRequests
{
    public class GetMaintenanceRequestsHandler : IRequestHandler<GetMaintenanceRequestsQuery, EndowmentResponse<IEnumerable<KeyValuPair>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetMaintenanceRequestsHandler(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
        public async Task<EndowmentResponse<IEnumerable<KeyValuPair>>> Handle(GetMaintenanceRequestsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.MaintenanceRequests.GetAllAsync(cancellationToken);
            var result = entities.Select(e => new KeyValuPair { Key = e.Id, Value = e.MaintenanceType });
            return Response.FilterResponse<IEnumerable<KeyValuPair>>(result);
        }
    }
} 