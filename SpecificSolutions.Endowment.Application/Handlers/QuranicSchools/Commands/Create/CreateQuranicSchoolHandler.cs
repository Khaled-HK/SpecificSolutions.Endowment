using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Buildings;
using SpecificSolutions.Endowment.Core.Entities.QuranicSchools;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Create
{
    public class CreateQuranicSchoolHandler : ICommandHandler<CreateQuranicSchoolCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateQuranicSchoolHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(CreateQuranicSchoolCommand request, CancellationToken cancellationToken)
        {
            var building = Building.Create(request);

            var quranicSchool = QuranicSchool.Create(building);
            await _unitOfWork.QuranicSchools.AddAsync(quranicSchool, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}