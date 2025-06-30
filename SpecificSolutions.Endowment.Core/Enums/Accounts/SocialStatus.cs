using System.ComponentModel;
namespace SpecificSolutions.Endowment.Core.Enums.Accounts;

public enum SocialStatus
{
    [Description("Married")]
    Married = 1,

    [Description("Widower")]
    Widower = 2,

    [Description("Divorced")]
    Divorced = 3,

    [Description("Single")]
    Single = 4,

    [Description("Child")]
    Child = 5,
}