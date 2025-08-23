using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Commands.RejectUser
{
    /// <summary>
    /// Command لرفض مستخدم وحذف حسابه
    /// </summary>
    public sealed record RejectUserCommand(string UserId) : ICommand;
}
