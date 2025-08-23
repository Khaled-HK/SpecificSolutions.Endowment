using SpecificSolutions.Endowment.Application.Abstractions.Contracts;
using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Resources;

namespace SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Commands.ApproveUser
{
    /// <summary>
    /// Handler للموافقة على المستخدمين - نمط خالد
    /// يستخدم IUserApprovalService لفصل منطق الأعمال عن Repository
    /// </summary>
    public class ApproveUserHandler : ICommandHandler<ApproveUserCommand>
    {
        private readonly IUserApprovalService _userApprovalService;
        private readonly ICurrentUser _currentUser;
        private readonly IApplicationRoleRepository _roleRepository;

        public ApproveUserHandler(
            IUserApprovalService userApprovalService,
            ICurrentUser currentUser,
            IApplicationRoleRepository roleRepository)
        {
            _userApprovalService = userApprovalService;
            _currentUser = currentUser;
            _roleRepository = roleRepository;
        }

        public async Task<EndowmentResponse> Handle(ApproveUserCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUser.GetUserIdOrDefault();
            if (!currentUserId.HasValue)
            {
                throw new UnauthorizedAccessException("يجب تسجيل الدخول لتنفيذ هذه العملية.");
            }

            // التحقق من وجود الدور في قاعدة البيانات - نمط خالد
            var roles = await _roleRepository.GetAllAsync(cancellationToken);
            var roleExists = roles.Any(r => string.Equals(r.Name, request.RoleName, StringComparison.OrdinalIgnoreCase));
            
            if (!roleExists)
            {
                return Response.FailureResponse("RoleName", $"الدور '{request.RoleName}' غير موجود في النظام");
            }

            // نمط خالد: Handler يستدعي Service فقط
            var result = await _userApprovalService.ApproveUserAsync(
                request.UserId, 
                request.RoleName, 
                cancellationToken);

            if (!result)
            {
                return Response.FailureResponse("ApprovalResult", "فشل في الموافقة على المستخدم");
            }

            return Response.SuccessResponse(ResponseState.Valid, "تم قبول المستخدم ومنحه الصلاحيات بنجاح");
        }
    }
}