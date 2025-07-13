using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Update
{
    public class UpdateRegionHandler : ICommandHandler<UpdateRegionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRegionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
        {
            var region = await _unitOfWork.Regions.GetByIdAsync(request.Id, cancellationToken);
            if (region == null)
                return Response.FailureResponse("Region not found.");

            //region.Name = request.Name;
            //region.Country = request.Country;

            await _unitOfWork.Regions.UpdateAsync(region, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}