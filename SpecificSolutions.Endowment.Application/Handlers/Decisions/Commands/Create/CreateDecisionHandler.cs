using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Decisions;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Create
{
    public class CreateDecisionHandler : ICommandHandler<CreateDecisionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;

        public CreateDecisionHandler(IUnitOfWork unitOfWork, IUserContext userContext)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<EndowmentResponse> Handle(CreateDecisionCommand request, CancellationToken cancellationToken)
        {
            // Get UserId from JWT token via IUserContext (آمن)
            var userId = _userContext.GetUserIdOrDefault();
            if (!userId.HasValue)
            {
                return Response.FailureResponse("User context is unavailable - Please log in again");
            }

            var decision = new Decision(request, userId.Value.ToString());

            await _unitOfWork.Decisions.AddAsync(decision, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}