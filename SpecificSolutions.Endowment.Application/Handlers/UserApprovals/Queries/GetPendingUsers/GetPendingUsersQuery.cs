using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.UserApprovals;

namespace SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Queries.GetPendingUsers
{
    /// <summary>
    /// Query للحصول على المستخدمين المعلقين (في انتظار الموافقة)
    /// </summary>
    public sealed record GetPendingUsersQuery : IQuery<List<PendingUserDto>>
    {
        public GetPendingUsersQuery(int pageNumber = 1, int pageSize = 10)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
