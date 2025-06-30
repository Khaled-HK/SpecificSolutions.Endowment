using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Commands.Delete
{
    public class DeleteConstructionRequestHandler : ICommandHandler<DeleteConstructionRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteConstructionRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(DeleteConstructionRequestCommand request, CancellationToken cancellationToken)
        {
            //var constructionRequest = await _constructionRequestRepository.GetByIdAsync(request.ConstructionRequestID);
            //if (constructionRequest == null) throw new ConstructionRequestNotFoundException();

            //await _constructionRequestRepository.DeleteAsync(request.ConstructionRequestID);

            return Response.Deleted();
        }
    }
}