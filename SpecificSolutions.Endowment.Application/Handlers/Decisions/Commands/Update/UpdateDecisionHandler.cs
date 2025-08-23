using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Update
{
    public class UpdateDecisionHandler : ICommandHandler<UpdateDecisionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public UpdateDecisionHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<EndowmentResponse> Handle(UpdateDecisionCommand request, CancellationToken cancellationToken)
        {
            var decision = await _unitOfWork.Decisions.GetByIdAsync(request.Id, cancellationToken);
            if (decision == null)
            {
                return Response.FailureResponse("Id", "القرار غير موجود");
            }

            // Get UserId from JWT token via ICurrentUser (للتحقق من الصلاحيات)
            var userId = _currentUser.GetUserIdOrDefault();
            if (!userId.HasValue)
            {
                return Response.FailureResponse("User context is unavailable - Please log in again");
            }

            // يمكن إضافة تحقق من الصلاحيات هنا إذا لزم الأمر
            // مثلاً: التحقق من أن المستخدم هو منشئ القرار أو لديه صلاحيات التحديث

            decision.Update(request);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Updated();
        }
    }
}