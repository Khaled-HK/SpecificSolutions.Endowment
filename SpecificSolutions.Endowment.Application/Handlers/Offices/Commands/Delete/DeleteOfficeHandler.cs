using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Delete
{
    public class DeleteOfficeHandler : IRequestHandler<DeleteOfficeCommand, EndowmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOfficeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = await _unitOfWork.Offices.GetByIdAsync(request.Id, cancellationToken);
            if (office == null)
                return Response.FailureResponse("Office not found.");

            // Check if office has any related data
            var hasRelatedData = await _unitOfWork.Offices.GetRelatedDataAsync(request.Id);
            if (hasRelatedData)
            {
                var errorMessage = $"Cannot delete office '{office.Name}' because it has related data. Please delete the related data first.";
                return Response.FailureResponse(errorMessage: errorMessage);
            }

            await _unitOfWork.Offices.RemoveAsync(office);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}