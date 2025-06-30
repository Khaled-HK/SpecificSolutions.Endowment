using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Abstractions.Requests
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizeAttribute : Attribute
    {
        public Permission Permissions { get; set; }
    }
}
