using Microsoft.Extensions.Logging;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Mosques;
using SpecificSolutions.Endowment.Core.Entities.Buildings;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Create
{
    public class CreateMosqueHandler : ICommandHandler<CreateMosqueCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<CreateMosqueHandler> _logger;

        public CreateMosqueHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, ILogger<CreateMosqueHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<EndowmentResponse> Handle(CreateMosqueCommand request, CancellationToken cancellationToken)
        {
            // Get UserId from JWT token via ICurrentUser and set it in the command
            var userId = _currentUser.GetUserIdOrDefault();
            if (!userId.HasValue)
            {
                return Response.FailureResponse("User context is unavailable - Please log in again");
            }

            try
            {
                // Set UserId in the command
                request.UserId = userId.Value.ToString();

                // Create the building first with UserId from token
                var building = Building.Create(request);
                await _unitOfWork.Buildings.AddAsync(building, cancellationToken);
                
                // Create the mosque with the existing building
                var mosque = Mosque.Create(request, building);
                await _unitOfWork.Mosques.AddAsync(mosque, cancellationToken);
                
                await _unitOfWork.CompleteAsync(cancellationToken);

                _logger.LogInformation("Mosque created successfully by user {UserId}", userId.Value);

                return Response.Added();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating mosque");
                throw;
            }
        }
    }
}