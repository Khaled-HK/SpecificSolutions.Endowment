using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Exceptions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Update
{
    public class UpdateQuranicSchoolHandler : ICommandHandler<UpdateQuranicSchoolCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateQuranicSchoolHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateQuranicSchoolCommand request, CancellationToken cancellationToken)
        {
            var quranicSchool = await _unitOfWork.QuranicSchools.GetByIdAsync(request.Id);
            if (quranicSchool == null) throw new QuranicSchoolNotFoundException();

            //quranicSchool.Update(
            //    request.SchoolName,
            //    // Other properties...
            //);

            await _unitOfWork.QuranicSchools.UpdateAsync(quranicSchool);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}