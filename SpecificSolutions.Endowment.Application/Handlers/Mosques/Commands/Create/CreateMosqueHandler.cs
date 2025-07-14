using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Mosques;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Create
{
    public class CreateMosqueHandler : ICommandHandler<CreateMosqueCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateMosqueHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateMosqueCommand request, CancellationToken cancellationToken)
        {
            var mosque = Mosque.Create(request);
            await _unitOfWork.Mosques.AddAsync(mosque, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}