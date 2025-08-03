using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Requests;
using SpecificSolutions.Endowment.Core.Entities.NeedsRequests;

namespace SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Commands.Create
{
    public class CreateNeedsRequestHandler : ICommandHandler<CreateNeedsRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateNeedsRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateNeedsRequestCommand request, CancellationToken cancellationToken)
        {
            // إنشاء Request أولاً
            var baseRequest = new Request(
                title: $"Needs Request: {request.NeedsType}",
                description: $"Needs Request for {request.NeedsType} at {request.Location}",
                referenceNumber: $"NR-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}",
                decisionId: Guid.Empty // سيتم تحديثه لاحقاً
            );

            await _unitOfWork.Requests.AddAsync(baseRequest, cancellationToken);

            // إنشاء NeedsRequest
            var needsRequest = NeedsRequest.Create(
                needsType: request.NeedsType,
                location: request.Location,
                estimatedCost: (double)request.EstimatedCost,
                provider: request.Provider,
                requestId: baseRequest.Id,
                request: baseRequest
            );

            await _unitOfWork.NeedsRequests.AddAsync(needsRequest, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}