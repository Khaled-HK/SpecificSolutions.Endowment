using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Commands.ApproveUser;
using SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Commands.RejectUser;
using SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Queries.GetPendingUsers;
using SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Queries.GetAvailableRoles;
using SpecificSolutions.Endowment.Application.Models.DTOs.UserApprovals;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Api.Controllers.UserApprovals
{
    /// <summary>
    /// Controller لإدارة موافقات المستخدمين - للمسؤولين فقط
    /// نمط خالد: CQRS + MediatR
    /// </summary>
    [Authorize]
    [Route("api/user-approvals")]
    public class UserApprovalController : ApiController
    {
        private readonly IMediator _mediator;
        public UserApprovalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// الحصول على المستخدمين المعلقين (في انتظار الموافقة)
        /// نمط خالد: Query + Handler
        /// </summary>
        [HttpGet("pending")]
        public async Task<EndowmentResponse<List<PendingUserDto>>> GetPendingUsers(
            [FromQuery] int page = 1,
            [FromQuery] int itemsPerPage = 10,
            CancellationToken cancellationToken = default)
            => await _mediator.Send(new GetPendingUsersQuery(page, itemsPerPage), cancellationToken);

        /// <summary>
        /// الموافقة على مستخدم ومنحه صلاحيات
        /// نمط خالد: Command + Handler
        /// </summary>
        [HttpPost("approve/{userId}")]
        public async Task<EndowmentResponse> ApproveUser(
            string userId,
            [FromBody] ApproveUserRequest request,
            CancellationToken cancellationToken = default)
            => await _mediator.Send(new ApproveUserCommand(userId, request.RoleName ?? "Employee"), cancellationToken);

        /// <summary>
        /// رفض مستخدم وحذف حسابه
        /// نمط خالد: Command + Handler
        /// </summary>
        [HttpDelete("reject/{userId}")]
        public async Task<EndowmentResponse> RejectUser(string userId, CancellationToken cancellationToken = default)
            => await _mediator.Send(new RejectUserCommand(userId), cancellationToken);

        /// <summary>
        /// الحصول على الأدوار المتاحة
        /// نمط خالد: Query + Handler
        /// </summary>
        [HttpGet("available-roles")]
        public async Task<EndowmentResponse<List<string>>> GetAvailableRoles(CancellationToken cancellationToken = default)
            => await _mediator.Send(new GetAvailableRolesQuery(), cancellationToken);
    }

    /// <summary>
    /// طلب الموافقة على المستخدم
    /// </summary>
    public class ApproveUserRequest
    {
        public string? RoleName { get; set; } = "Employee";
    }
}
