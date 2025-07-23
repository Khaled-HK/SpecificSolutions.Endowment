using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Exceptions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Update
{
    public class UpdateMosqueHandler : ICommandHandler<UpdateMosqueCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;

        public UpdateMosqueHandler(IUnitOfWork unitOfWork, IUserContext userContext)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<EndowmentResponse> Handle(UpdateMosqueCommand request, CancellationToken cancellationToken)
        {
            var mosque = await _unitOfWork.Mosques.GetByIdAsync(request.Id, cancellationToken);
            if (mosque == null) throw new MosqueNotFoundException();

            // Get UserId from JWT token via IUserContext and set it in the command for audit purposes
            var userId = _userContext.GetUserIdOrDefault();
            if (!userId.HasValue)
            {
                return Response.FailureResponse("User context is unavailable - Please log in again");
            }
            
            request.UserId = userId.Value.ToString();

            // Update the mosque
            mosque.Update(request);

            // Update the building as well
            mosque.Building.Update(request);

            // طباعة للتأكد من التحديث
            Console.WriteLine($"Mosque updated - Definition: {mosque.MosqueDefinition}, Classification: {mosque.MosqueClassification}");
            Console.WriteLine($"Building updated - Name: {mosque.Building.Name}, RegionId: {mosque.Building.RegionId}, OfficeId: {mosque.Building.OfficeId}");
            Console.WriteLine($"Dates updated - OpeningDate: {mosque.Building.OpeningDate}, ConstructionDate: {mosque.Building.ConstructionDate}");

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}