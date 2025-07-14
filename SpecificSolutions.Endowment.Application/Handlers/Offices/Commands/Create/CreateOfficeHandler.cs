using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Offices;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Create
{
    public class CreateOfficeHandler : IRequestHandler<CreateOfficeCommand, EndowmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOfficeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = new Office(request.Name, request.Location, request.PhoneNumber, request.RegionId);
            await _unitOfWork.Offices.AddAsync(office, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Response.Added();
        }
    }
}