using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.UserApprovals;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Queries.GetPendingUsers
{
    /// <summary>
    /// Handler لجلب المستخدمين المعلقين - نمط خالد
    /// يستدعي Repository مباشرة لجلب البيانات
    /// </summary>
    public class GetPendingUsersHandler : IQueryHandler<GetPendingUsersQuery, List<PendingUserDto>>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public GetPendingUsersHandler(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<EndowmentResponse<List<PendingUserDto>>> Handle(GetPendingUsersQuery request, CancellationToken cancellationToken)
        {
            // نمط خالد: Handler يستدعي Repository مباشرة لجلب البيانات
            var pendingUsers = await _applicationUserRepository.GetPendingUsersAsync(
                request.PageNumber, 
                request.PageSize, 
                cancellationToken);

            if (!pendingUsers.Any())
                return Response.FilterResponse<List<PendingUserDto>>(new List<PendingUserDto>());

            return Response.FilterResponse<List<PendingUserDto>>(pendingUsers);
        }
    }
}
