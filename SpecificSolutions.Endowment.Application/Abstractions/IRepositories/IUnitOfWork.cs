using SpecificSolutions.Endowment.Application.Contracts.IRepositories;

namespace SpecificSolutions.Endowment.Application.Abstractions.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRequestRepository RequestRepository { get; }
        IDecisionRepository DecisionRepository { get; }
        IAccountRepository AccountRepository { get; }
        IAccountDetailRepository AccountDetailRepository { get; }
        IOfficeRepository OfficeRepository { get; }
        ILoginRepository LoginRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IAuditLogsRepository AuditLogsRepository { get; }
        IApplicationRoleRepository ApplicationRoleRepository { get; }
        IApplicationUserRepository ApplicationUserRepository { get; }
        IApplicationUserRolesRepository ApplicationUserRolesRepository { get; }
        IMosqueRepository Mosques { get; }
        IConstructionRequestRepository ConstructionRequests { get; }
        IMaintenanceRequestRepository MaintenanceRequests { get; }
        IChangeOfPathRequestRepository ChangeOfPathRequests { get; }
        INameChangeRequestRepository NameChangeRequests { get; }
        IExpenditureChangeRequestRepository ExpenditureChangeRequests { get; }
        INeedsRequestRepository NeedsRequests { get; }
        IDemolitionRequestRepository DemolitionRequests { get; }
        IQuranicSchoolRepository QuranicSchools { get; }
        IBuildingRepository Buildings { get; }
        IFacilityDetailRepository FacilityDetails { get; }
        IFacilityRepository Facilities { get; }
        IBuildingDetailRepository BuildingDetails { get; }
        IBankRepository Banks { get; }
        ICityRepository Cities { get; }
        IBranchRepository Branches { get; } // Add Branches repository
        IProductRepository Products { get; } // Add Products repository
        IRegionRepository Regions { get; } // Add Regions repository
                IBuildingDetailRequestRepository BuildingDetailRequests { get; }

        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    }
}
