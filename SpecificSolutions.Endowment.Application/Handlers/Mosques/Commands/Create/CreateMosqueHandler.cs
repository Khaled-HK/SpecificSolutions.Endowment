using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Buildings;
using SpecificSolutions.Endowment.Core.Entities.Mosques;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Create
{
    public class CreateMosqueHandler : ICommandHandler<CreateMosqueCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;

        public CreateMosqueHandler(IUnitOfWork unitOfWork, IUserContext userContext)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<EndowmentResponse> Handle(CreateMosqueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get UserId from JWT token via IUserContext and set it in the command
                var userId = _userContext.GetUserIdOrDefault();
                if (!userId.HasValue)
                {
                    return Response.FailureResponse("User context is unavailable - Please log in again");
                }
                
                request.UserId = userId.Value.ToString();

                // Create the building first with UserId from token
                var building = Building.Create(request);
                await _unitOfWork.Buildings.AddAsync(building, cancellationToken);
                
                // Create the mosque with the existing building
                var mosque = Mosque.Create(request, building);
                await _unitOfWork.Mosques.AddAsync(mosque, cancellationToken);
                
                await _unitOfWork.CompleteAsync(cancellationToken);

                return Response.Added();
            }
            catch (Exception ex)
            {
                return Response.FailureResponse($"حدث خطأ أثناء إنشاء المسجد: {ex.Message}");
            }
        }
    }
}