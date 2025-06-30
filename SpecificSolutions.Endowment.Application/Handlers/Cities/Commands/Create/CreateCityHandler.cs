using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Cities;

namespace SpecificSolutions.Endowment.Application.Handlers.Cities.Commands.Create
{
    public class CreateCityHandler : ICommandHandler<CreateCityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            var city = City.Create(request.Name, request.Country);

            await _unitOfWork.Cities.AddAsync(city);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}