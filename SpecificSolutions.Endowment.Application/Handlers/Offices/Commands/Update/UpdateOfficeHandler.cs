using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Exceptions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Update
{
    public class UpdateOfficeHandler : IRequestHandler<UpdateOfficeCommand, EndowmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateOfficeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = await _unitOfWork.Offices.GetByIdAsync(request.Id);
            if (office == null)
            {
                throw new OfficeNotFoundException(request.Id);
            }

            office.Update(request.Name, request.Location, request.PhoneNumber);
            await _unitOfWork.Offices.UpdateAsync(office);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();

        }
    }
}