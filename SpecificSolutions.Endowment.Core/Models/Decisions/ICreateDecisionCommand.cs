namespace SpecificSolutions.Endowment.Core.Models.Decisions
{
    public interface ICreateDecisionCommand
    {
        string Title { get; set; }
        string Description { get; set; }
        DateTime CreatedDate { get; set; }
        string ReferenceNumber { get; set; }
        // تم إزالة UserId لمنع انتحال الشخصية - سيتم تعيينه من JWT token
    }
}
