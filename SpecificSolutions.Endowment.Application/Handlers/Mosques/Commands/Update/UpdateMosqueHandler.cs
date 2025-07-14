using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Handlers.Mosques.Exceptions;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Update
{
    public class UpdateMosqueHandler : ICommandHandler<UpdateMosqueCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMosqueHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateMosqueCommand request, CancellationToken cancellationToken)
        {
            var mosque = await _unitOfWork.Mosques.GetByIdAsync(request.Id, cancellationToken);
            if (mosque == null) throw new MosqueNotFoundException();

            mosque.Update(request);

            //await _unitOfWork.Mosques.UpdateAsync(mosque);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}