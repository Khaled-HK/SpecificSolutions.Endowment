using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.UserApprovals.Queries.GetAvailableRoles
{
    /// <summary>
    /// Handler لجلب الأدوار المتاحة من قاعدة البيانات - نمط خالد
    /// </summary>
    public class GetAvailableRolesHandler : IQueryHandler<GetAvailableRolesQuery, List<string>>
    {
        private readonly IApplicationRoleRepository _roleRepository;

        public GetAvailableRolesHandler(IApplicationRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<EndowmentResponse<List<string>>> Handle(GetAvailableRolesQuery request, CancellationToken cancellationToken)
        {
            // نمط خالد: جلب الأدوار من قاعدة البيانات
            var roles = await _roleRepository.GetAllAsync(cancellationToken);
            var roleNames = roles.Select(r => r.Name).ToList();

            return Response.FilterResponse<List<string>>(roleNames);
        }
    }
}
