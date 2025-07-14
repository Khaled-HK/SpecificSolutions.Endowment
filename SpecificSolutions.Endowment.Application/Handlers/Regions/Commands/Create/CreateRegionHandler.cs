using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Regions;

namespace SpecificSolutions.Endowment.Application.Handlers.Regions.Commands.Create
{
    public class CreateRegionHandler : ICommandHandler<CreateRegionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateRegionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateRegionCommand request, CancellationToken cancellationToken)
        {
            var region = Region.Create(request.Name, request.Country, request.CityId);

            await _unitOfWork.Regions.AddAsync(region, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}