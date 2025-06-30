using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Handlers.Offices.Exceptions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Delete
{
    public class DeleteOfficeHandler : IRequestHandler<DeleteOfficeCommand, EndowmentResponse>
    {
        private readonly IOfficeRepository _officeRepository;

        public DeleteOfficeHandler(IOfficeRepository officeRepository)
        {
            _officeRepository = officeRepository;
        }

        public async Task<EndowmentResponse> Handle(DeleteOfficeCommand request, CancellationToken cancellationToken)
        {
            var office = await _officeRepository.GetByIdAsync(request.Id);
            if (office == null)
            {
                throw new OfficeNotFoundException(request.Id);
            }

            await _officeRepository.RemoveAsync(office);

            return Response.Deleted();
        }
    }
}