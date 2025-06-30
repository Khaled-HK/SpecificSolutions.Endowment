using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Create
{
    public class CreateChangeOfPathRequestHandler : ICommandHandler<CreateChangeOfPathRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateChangeOfPathRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateChangeOfPathRequestCommand request, CancellationToken cancellationToken)
        {
            //var changeOfPathRequest = new ChangeOfPathRequest
            //{
            //    RequestType = request.ChangeOfPathRequest.RequestType,
            //    SubmissionDate = request.ChangeOfPathRequest.SubmissionDate,
            //    RequestStatus = request.ChangeOfPathRequest.RequestStatus,
            //    Attachments = request.ChangeOfPathRequest.Attachments,
            //    CurrentType = request.ChangeOfPathRequest.CurrentType,
            //    NewType = request.ChangeOfPathRequest.NewType,
            //    Reason = request.ChangeOfPathRequest.Reason
            //};

            //// Add the request to the repository
            //await _unitOfWork.ChangeOfPathRequests.AddAsync(request);

            //// Commit the transaction
            //await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();

        }
    }
}