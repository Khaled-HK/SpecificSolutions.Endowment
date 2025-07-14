using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Exceptions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Delete
{
    public class DeleteQuranicSchoolHandler : ICommandHandler<DeleteQuranicSchoolCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuranicSchoolHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteQuranicSchoolCommand request, CancellationToken cancellationToken)
        {
            var quranicSchool = await _unitOfWork.QuranicSchools.GetByIdAsync(request.Id, cancellationToken);
            if (quranicSchool == null) throw new QuranicSchoolNotFoundException();

            await _unitOfWork.QuranicSchools.RemoveAsync(quranicSchool);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}