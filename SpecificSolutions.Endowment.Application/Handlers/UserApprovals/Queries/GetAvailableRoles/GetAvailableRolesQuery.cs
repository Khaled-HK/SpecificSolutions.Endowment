using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Queries.GetAvailableRoles
{
    /// <summary>
    /// Query لجلب الأدوار المتاحة - نمط خالد
    /// </summary>
    public sealed record GetAvailableRolesQuery : IQuery<List<string>>
    {
        // خصائص مطلوبة من IQuery حتى لو لم تُستخدم هنا
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
