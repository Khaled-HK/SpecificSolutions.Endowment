using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Delete
{
    public class DeleteCityHandler : ICommandHandler<DeleteCityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            var city = await _unitOfWork.Cities.GetByIdAsync(request.Id, cancellationToken);
            if (city == null)
                return Response.FailureResponse("City not found.");

            await _unitOfWork.Cities.DeleteAsync(request.Id);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}