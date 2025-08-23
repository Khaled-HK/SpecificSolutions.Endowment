using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Update
{
    public class UpdateMosqueHandler : ICommandHandler<UpdateMosqueCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public UpdateMosqueHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<EndowmentResponse> Handle(UpdateMosqueCommand request, CancellationToken cancellationToken)
        {
            var mosque = await _unitOfWork.Mosques.GetByIdAsync(request.Id, cancellationToken);
            if (mosque == null)
            {
                return Response.FailureResponse("Id", "المسجد غير موجود");
            }

            // Get UserId from JWT token via ICurrentUser and set it in the command for audit purposes
            var userId = _currentUser.GetUserIdOrDefault();
            if (!userId.HasValue)
            {
                return Response.FailureResponse("User context is unavailable - Please log in again");
            }

            mosque.Update(request);

            // Update the building as well
            //mosque.Building.Update(request);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}