namespace SpecificSolutions.Endowment.Core.Models.Decisions
{
    public interface IUpdateDecisionCommand
    {
        string Title { get; set; }
        string Description { get; set; }
        string ReferenceNumber { get; set; }
        // تم إزالة UserId لمنع انتحال الشخصية - سيتم تعيينه من JWT token
    }
}
