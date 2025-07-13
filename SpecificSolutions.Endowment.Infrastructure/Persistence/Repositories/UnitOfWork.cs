using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Contracts.IRepositories;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.AccountDetails;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Accounts;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.ApplicationRoles;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.ApplicationUserRoles;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.ApplicationUsers;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.AuditLogs;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Banks;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Branchs;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.BuildingDetailRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.BuildingDetails;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Buildings;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.ChangeOfPathRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Cities;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.ConstructionRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Decisions;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.DemolitionRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.EndowmentExpenditureChangeRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Facilities;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.FacilityDetails;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Login;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.MaintenanceRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Mosques;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.NameChangeRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.NeedsRequests;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Offices;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.QuranicSchools;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.RefreshTokens;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Regions;
using SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories.Requests;

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private IRequestRepository _requests;
        private IDecisionRepository _decisions;
        private IAccountRepository _accounts;
        private IAccountDetailRepository _accountDetails;
        private IOfficeRepository _Offices;
        private ILoginRepository _logins;
        private IRefreshTokenRepository _refreshTokens;
        private IAuditLogsRepository _auditLogs;
        private IApplicationRoleRepository _applicationRoles;
        private IApplicationUserRepository _applicationUsers;
        private IApplicationUserRolesRepository _applicationUserRoles;
        private IMosqueRepository _mosques;
        private IConstructionRequestRepository _constructionRequests;
        private IMaintenanceRequestRepository _maintenanceRequests;
        private IChangeOfPathRequestRepository _changeOfPathRequests;
        private INameChangeRequestRepository _nameChangeRequests;
        private IExpenditureChangeRequestRepository _endowmentExpenditureChangeRequests;
        private INeedsRequestRepository _needsRequests;
        private IDemolitionRequestRepository _DemolitionRequests;
        private IQuranicSchoolRepository _quranicSchools;
        private IBuildingRepository _buildings;
        private IFacilityRepository _facilities;
        private IBuildingDetailRepository _buildingDetailRepository;
        private ICityRepository _cityRepository;
        private IRegionRepository _regions;
        private IProductRepository _products;
        private IBankRepository _banks;
        private IBranchRepository _branchs;
        private IFacilityDetailRepository _FacilityDetails;
        private IBuildingDetailRequestRepository _buildingDetailRequestRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRequestRepository Requests => _requests ??= new RequestRepository(_context);
        public IDecisionRepository Decisions => _decisions ??= new DecisionRepository(_context);
        public IAccountRepository Accounts => _accounts ??= new AccountRepository(_context);
        public IAccountDetailRepository AccountDetails => _accountDetails ??= new AccountDetailRepository(_context);
        public IOfficeRepository Offices => _Offices ??= new OfficeRepository(_context);
        public ILoginRepository Logins => _logins ??= new LoginRepository(_context);
        public IRefreshTokenRepository RefreshTokens => _refreshTokens ??= new RefreshTokenRepository(_context);
        public IAuditLogsRepository AuditLogs => _auditLogs ??= new AuditLogsRepository(_context);
        public IApplicationRoleRepository ApplicationRoles => _applicationRoles ??= new ApplicationRoleRepository(_context);
        public IApplicationUserRepository ApplicationUsers => _applicationUsers ??= new ApplicationUserRepository(_context);
        public IApplicationUserRolesRepository ApplicationUserRoles => _applicationUserRoles ??= new ApplicationUserRolesRepository(_context);
        public IMosqueRepository Mosques => _mosques ??= new MosqueRepository(_context);
        public IConstructionRequestRepository ConstructionRequests => _constructionRequests ??= new ConstructionRequestRepository(_context);
        public IMaintenanceRequestRepository MaintenanceRequests => _maintenanceRequests ??= new MaintenanceRequestRepository(_context);
        public IChangeOfPathRequestRepository ChangeOfPathRequests => _changeOfPathRequests ??= new ChangeOfPathRequestRepository(_context);
        public INameChangeRequestRepository NameChangeRequests => _nameChangeRequests ??= new NameChangeRequestRepository(_context);
        public IExpenditureChangeRequestRepository ExpenditureChangeRequests => _endowmentExpenditureChangeRequests ??= new ExpenditureChangeRequestRepository(_context);
        public INeedsRequestRepository NeedsRequests => _needsRequests ??= new NeedsRequestRepository(_context);
        public IDemolitionRequestRepository DemolitionRequests => _DemolitionRequests ??= new DemolitionRequestRepository(_context);
        public IQuranicSchoolRepository QuranicSchools => _quranicSchools ??= new QuranicSchoolRepository(_context);
        public IBuildingRepository Buildings => _buildings ??= new BuildingRepository(_context);
        public IFacilityRepository Facilities => _facilities ??= new FacilityRepository(_context);
        public IBuildingDetailRepository BuildingDetails => _buildingDetailRepository ??= new BuildingDetailRepository(_context);
        public ICityRepository Cities => _cityRepository ??= new CityRepository(_context);
        public IRegionRepository Regions => _regions ??= new RegionRepository(_context);
        public IProductRepository Products => _products ??= new ProductRepository(_context);
        public IBankRepository Banks => _banks ??= new BankRepository(_context);
        public IBranchRepository Branches => _branchs ??= new BranchRepository(_context);
        public IFacilityDetailRepository FacilityDetails => _FacilityDetails ??= new FacilityDetailRepository(_context);
        public IBuildingDetailRequestRepository BuildingDetailRequests => _buildingDetailRequestRepository ??= new BuildingDetailRequestRepository(_context);

        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}