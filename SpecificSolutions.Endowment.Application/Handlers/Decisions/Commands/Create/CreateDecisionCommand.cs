using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Models.Decisions;

namespace SpecificSolutions.Endowment.Application.Handlers.Decisions.Commands.Create
{
    [Authorize(Permissions = Permission.DecisionAdd)]
    public class CreateDecisionCommand : ICommand, ICreateDecisionCommand
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // إعادة إضافة مع قيمة افتراضية
        public string ReferenceNumber { get; set; }
        // تم إزالة UserId لمنع انتحال الشخصية - سيتم تعيينه من JWT token
    }
}