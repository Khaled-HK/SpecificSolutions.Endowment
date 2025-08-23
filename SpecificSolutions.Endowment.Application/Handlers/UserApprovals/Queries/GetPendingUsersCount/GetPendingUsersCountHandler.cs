using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Queries.GetPendingUsersCount
{
    /// <summary>
    /// Handler لجلب عدد المستخدمين المعلقين - نمط خالد
    /// يستدعي Repository مباشرة لجلب البيانات مع الفلترة
    /// </summary>
    public class GetPendingUsersCountHandler : IQueryHandler<GetPendingUsersCountQuery, int>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public GetPendingUsersCountHandler(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<EndowmentResponse<int>> Handle(GetPendingUsersCountQuery request, CancellationToken cancellationToken)
        {
            // نمط خالد: Handler يستدعي Repository مباشرة لجلب البيانات مع الفلترة
            var count = await _applicationUserRepository.GetPendingUsersCountAsync(
                request.SearchTerm,
                request.OfficeId,
                request.FromDate,
                request.ToDate,
                cancellationToken);

            return Response.FilterResponse<int>(count);
        }
    }
}
