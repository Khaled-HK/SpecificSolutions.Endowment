using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Commands.RejectUser
{
    /// <summary>
    /// Handler لرفض المستخدمين - نمط خالد
    /// يستخدم IUserApprovalService لفصل منطق الأعمال عن Repository
    /// </summary>
    public class RejectUserHandler : ICommandHandler<RejectUserCommand>
    {
        private readonly IUserApprovalService _userApprovalService;
        private readonly ICurrentUser _currentUser;

        public RejectUserHandler(
            IUserApprovalService userApprovalService,
            ICurrentUser currentUser)
        {
            _userApprovalService = userApprovalService;
            _currentUser = currentUser;
        }

        public async Task<EndowmentResponse> Handle(RejectUserCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUser.GetUserIdOrDefault();
            if (!currentUserId.HasValue)
            {
                throw new UnauthorizedAccessException("يجب تسجيل الدخول لتنفيذ هذه العملية.");
            }

            // نمط خالد: Handler يستدعي Service فقط
            var result = await _userApprovalService.RejectUserAsync(
                request.UserId, 
                cancellationToken);

            if (!result)
            {
                return Response.FailureResponse("RejectResult", "فشل في رفض المستخدم");
            }

            return Response.SuccessResponse(ResponseState.Valid, "تم رفض المستخدم وحذف حسابه بنجاح");
        }
    }
}