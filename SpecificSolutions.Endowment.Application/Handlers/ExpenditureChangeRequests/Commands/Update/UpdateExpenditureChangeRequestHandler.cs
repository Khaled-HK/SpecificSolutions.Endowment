using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Commands.Update
{
    public class UpdateExpenditureChangeRequestHandler : ICommandHandler<UpdateExpenditureChangeRequestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateExpenditureChangeRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EndowmentResponse> Handle(UpdateExpenditureChangeRequestCommand request, CancellationToken cancellationToken)
        {
            return Response.Updated();
        }
    }
}