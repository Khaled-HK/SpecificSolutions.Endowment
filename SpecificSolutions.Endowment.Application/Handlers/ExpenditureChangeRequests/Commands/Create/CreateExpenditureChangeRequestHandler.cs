using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Create
{
    public class CreateExpenditureChangeRequestHandler : ICommandHandler<CreateExpenditureChangeRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateExpenditureChangeRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateExpenditureChangeRequestCommand request, CancellationToken cancellationToken)
        {
            //var expenditureChangeRequest = new EndowmentExpenditureChangeRequest
            //{
            //    EndowmentId = request.EndowmentId,
            //    RequestedAmount = request.RequestedAmount,
            //    RequestedDate = request.RequestedDate,
            //    RequestedReason = request.RequestedReason,
            //    RequestedStatus = request.RequestedStatus
            //};
            //await _unitOfWork.EndowmentExpenditureChangeRequestRepository.AddAsync(expenditureChangeRequest);
            //await _unitOfWork.CompleteAsync(cancellationToken);
            return Response.Added();
        }
    }
}