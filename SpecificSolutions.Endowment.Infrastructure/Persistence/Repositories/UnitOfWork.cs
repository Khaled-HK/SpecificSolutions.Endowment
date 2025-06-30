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

        private IRequestRepository _requestRepository;
        private IDecisionRepository _decisionRepository;
        private IAccountRepository _accountRepository;
        private IAccountDetailRepository _accountDetailRepository;
        private IOfficeRepository _OfficeRepository;
        private ILoginRepository _loginRepository;
        private IRefreshTokenRepository _refreshTokenRepository;
        private IAuditLogsRepository _auditLogsRepository;
        private IApplicationRoleRepository _applicationRoleRepository;
        private IApplicationUserRepository _applicationUserRepository;
        private IApplicationUserRolesRepository _applicationUserRolesRepository;
        private IMosqueRepository _mosqueRepository;
        private IConstructionRequestRepository _constructionRequestRepository;
        private IMaintenanceRequestRepository _maintenanceRequestRepository;
        private IChangeOfPathRequestRepository _changeOfPathRequestRepository;
        private INameChangeRequestRepository _nameChangeRequestRepository;
        private IExpenditureChangeRequestRepository _endowmentExpenditureChangeRequestRepository;
        private INeedsRequestRepository _needsRequestRepository;
        private IDemolitionRequestRepository _DemolitionRequestRepository;
        private IQuranicSchoolRepository _quranicSchoolRepository;
        private IBuildingRepository _buildings;
        private IFacilityRepository _facilityRepository;
        private IBuildingDetailRepository _buildingDetailRepository;
        private ICityRepository _cityRepository;
        private IRegionRepository _regionRepository;
        private IProductRepository _productRepository;
        private IBankRepository _bankRepository;
        private IBranchRepository _branchRepository;
        private IFacilityDetailRepository _FacilityDetails;
        private IBuildingDetailRequestRepository _buildingDetailRequestRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRequestRepository RequestRepository => _requestRepository ??= new RequestRepository(_context);
        public IDecisionRepository DecisionRepository => _decisionRepository ??= new DecisionRepository(_context);
        public IAccountRepository AccountRepository => _accountRepository ??= new AccountRepository(_context);
        public IAccountDetailRepository AccountDetailRepository => _accountDetailRepository ??= new AccountDetailRepository(_context);
        public IOfficeRepository OfficeRepository => _OfficeRepository ??= new OfficeRepository(_context);
        public ILoginRepository LoginRepository => _loginRepository ??= new LoginRepository(_context);
        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ??= new RefreshTokenRepository(_context);
        public IAuditLogsRepository AuditLogsRepository => _auditLogsRepository ??= new AuditLogsRepository(_context);
        public IApplicationRoleRepository ApplicationRoleRepository => _applicationRoleRepository ??= new ApplicationRoleRepository(_context);
        public IApplicationUserRepository ApplicationUserRepository => _applicationUserRepository ??= new ApplicationUserRepository(_context);
        public IApplicationUserRolesRepository ApplicationUserRolesRepository => _applicationUserRolesRepository ??= new ApplicationUserRolesRepository(_context);
        public IMosqueRepository Mosques => _mosqueRepository ??= new MosqueRepository(_context);
        public IConstructionRequestRepository ConstructionRequests => _constructionRequestRepository ??= new ConstructionRequestRepository(_context);
        public IMaintenanceRequestRepository MaintenanceRequests => _maintenanceRequestRepository ??= new MaintenanceRequestRepository(_context);
        public IChangeOfPathRequestRepository ChangeOfPathRequests => _changeOfPathRequestRepository ??= new ChangeOfPathRequestRepository(_context);
        public INameChangeRequestRepository NameChangeRequests => _nameChangeRequestRepository ??= new NameChangeRequestRepository(_context);
        public IExpenditureChangeRequestRepository ExpenditureChangeRequests => _endowmentExpenditureChangeRequestRepository ??= new ExpenditureChangeRequestRepository(_context);
        public INeedsRequestRepository NeedsRequests => _needsRequestRepository ??= new NeedsRequestRepository(_context);
        public IDemolitionRequestRepository DemolitionRequests => _DemolitionRequestRepository ??= new DemolitionRequestRepository(_context);
        public IQuranicSchoolRepository QuranicSchools => _quranicSchoolRepository ??= new QuranicSchoolRepository(_context);
        public IBuildingRepository Buildings => _buildings ??= new BuildingRepository(_context);
        public IFacilityRepository Facilities => _facilityRepository ??= new FacilityRepository(_context);
        public IBuildingDetailRepository BuildingDetails => _buildingDetailRepository ??= new BuildingDetailRepository(_context);
        public ICityRepository Cities => _cityRepository ??= new CityRepository(_context);
        public IRegionRepository Regions => _regionRepository ??= new RegionRepository(_context);
        public IProductRepository Products => _productRepository ??= new ProductRepository(_context);
        public IBankRepository Banks => _bankRepository ??= new BankRepository(_context);
        public IBranchRepository Branches => _branchRepository ??= new BranchRepository(_context);
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