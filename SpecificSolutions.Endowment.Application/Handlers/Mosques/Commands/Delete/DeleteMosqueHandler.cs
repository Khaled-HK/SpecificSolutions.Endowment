using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Exceptions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Delete
{
    public class DeleteMosqueHandler : ICommandHandler<DeleteMosqueCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMosqueHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteMosqueCommand request, CancellationToken cancellationToken)
        {
            var mosque = await _unitOfWork.Mosques.GetByIdAsync(request.Id);
            if (mosque == null) throw new MosqueNotFoundException();

            await _unitOfWork.Mosques.DeleteAsync(request.Id);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Deleted();
        }
    }
}