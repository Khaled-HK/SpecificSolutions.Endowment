using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Commands.ApproveUser
{
    /// <summary>
    /// Command للموافقة على مستخدم ومنحه صلاحيات
    /// </summary>
    public sealed record ApproveUserCommand(
        string UserId,
        string RoleName = "Employee"
    ) : ICommand;
}
