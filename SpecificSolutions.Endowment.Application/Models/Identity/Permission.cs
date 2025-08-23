using System.Text.Json.Serialization;

namespace SpecificSolutions.Endowment.Application.Models.Identity
{
    public sealed class Permission
    {
        public bool AccountView { get; set; }
        public bool AccountAdd { get; set; }
        public bool AccountEdit { get; set; }
        public bool AccountDelete { get; set; }

        // AccountDetail Permissions
        public bool AccountDetailView { get; set; }
        public bool AccountDetailAdd { get; set; }
        public bool AccountDetailEdit { get; set; }
        public bool AccountDetailDelete { get; set; }

        public bool UserView { get; set; }
        public bool UserAdd { get; set; }
        public bool UserEdit { get; set; }
        public bool UserDelete { get; set; }

        public bool RoleView { get; set; }
        public bool RoleAdd { get; set; }
        public bool RoleEdit { get; set; }
        public bool RoleDelete { get; set; }

        public bool DecisionView { get; set; }
        public bool DecisionAdd { get; set; }
        public bool DecisionEdit { get; set; }
        public bool DecisionDelete { get; set; }

        public bool RequestView { get; set; }
        public bool RequestAdd { get; set; }
        public bool RequestEdit { get; set; }
        public bool RequestDelete { get; set; }

        // ConstructionRequest Permissions
        public bool ConstructionRequestView { get; set; }
        public bool ConstructionRequestAdd { get; set; }
        public bool ConstructionRequestEdit { get; set; }
        public bool ConstructionRequestDelete { get; set; }

        // MaintenanceRequest Permissions
        public bool MaintenanceRequestView { get; set; }
        public bool MaintenanceRequestAdd { get; set; }
        public bool MaintenanceRequestEdit { get; set; }
        public bool MaintenanceRequestDelete { get; set; }

        // ChangeOfPathRequest Permissions
        public bool ChangeOfPathRequestView { get; set; }
        public bool ChangeOfPathRequestAdd { get; set; }
        public bool ChangeOfPathRequestEdit { get; set; }
        public bool ChangeOfPathRequestDelete { get; set; }

        // DemolitionRequest Permissions
        public bool DemolitionRequestView { get; set; }
        public bool DemolitionRequestAdd { get; set; }
        public bool DemolitionRequestEdit { get; set; }
        public bool DemolitionRequestDelete { get; set; }

        // NameChangeRequest Permissions
        public bool NameChangeRequestView { get; set; }
        public bool NameChangeRequestAdd { get; set; }
        public bool NameChangeRequestEdit { get; set; }
        public bool NameChangeRequestDelete { get; set; }

        // NeedsRequest Permissions
        public bool NeedsRequestView { get; set; }
        public bool NeedsRequestAdd { get; set; }
        public bool NeedsRequestEdit { get; set; }
        public bool NeedsRequestDelete { get; set; }

        // ExpenditureChangeRequest Permissions
        public bool ExpenditureChangeRequestView { get; set; }
        public bool ExpenditureChangeRequestAdd { get; set; }
        public bool ExpenditureChangeRequestEdit { get; set; }
        public bool ExpenditureChangeRequestDelete { get; set; }

        

        public bool OfficeView { get; set; }
        public bool OfficeAdd { get; set; }
        public bool OfficeEdit { get; set; }
        public bool OfficeDelete { get; set; }

        public bool EndowmentView { get; set; }
        public bool EndowmentAdd { get; set; }
        public bool EndowmentEdit { get; set; }
        public bool EndowmentDelete { get; set; }

        //City Permissions
        public bool CityView { get; set; }
        public bool CityAdd { get; set; }
        public bool CityEdit { get; set; }
        public bool CityDelete { get; set; }

        //Region Permissions
        public bool RegionView { get; set; }
        public bool RegionAdd { get; set; }
        public bool RegionEdit { get; set; }
        public bool RegionDelete { get; set; }

        //Buildings Permissions
        public bool BuildingView { get; set; }
        public bool BuildingAdd { get; set; }
        public bool BuildingEdit { get; set; }
        public bool BuildingDelete { get; set; }

        //Mosque Permissions
        public bool MosqueView { get; set; }
        public bool MosqueAdd { get; set; }
        public bool MosqueEdit { get; set; }
        public bool MosqueDelete { get; set; }

        //Product Permissions
        public bool ProductView { get; set; }
        public bool ProductAdd { get; set; }
        public bool ProductEdit { get; set; }
        public bool ProductDelete { get; set; }

        //Bank Permissions
        public bool BankView { get; set; }
        public bool BankAdd { get; set; }
        public bool BankEdit { get; set; }
        public bool BankDelete { get; set; }

        //Branch Permissions
        public bool BranchView { get; set; }
        public bool BranchAdd { get; set; }
        public bool BranchEdit { get; set; }
        public bool BranchDelete { get; set; }

        //Facility Permissions
        public bool FacilityView { get; set; }
        public bool FacilityAdd { get; set; }
        public bool FacilityEdit { get; set; }
        public bool FacilityDelete { get; set; }

        //BuildingDetailRequest Permissions
        public bool BuildingDetailRequestView { get; set; }
        public bool BuildingDetailRequestAdd { get; set; }
        public bool BuildingDetailRequestEdit { get; set; }
        public bool BuildingDetailRequestDelete { get; set; }

        //QuranicSchool Permissions
        public bool QuranicSchoolView { get; set; }
        public bool QuranicSchoolAdd { get; set; }
        public bool QuranicSchoolEdit { get; set; }
        public bool QuranicSchoolDelete { get; set; }

        // Public parameterless constructor for JSON deserialization
        public Permission() { }

        // JsonConstructor for parameterized constructor
        [JsonConstructor]
        public Permission(
            bool accountView, bool accountAdd, bool accountEdit, bool accountDelete,
            bool accountDetailView, bool accountDetailAdd, bool accountDetailEdit, bool accountDetailDelete,
            bool userView, bool userAdd, bool userEdit, bool userDelete,
            bool roleView, bool roleAdd, bool roleEdit, bool roleDelete,
            bool decisionView, bool decisionAdd, bool decisionEdit, bool decisionDelete,
            bool requestView, bool requestAdd, bool requestEdit, bool requestDelete,
            bool constructionRequestView, bool constructionRequestAdd, bool constructionRequestEdit, bool constructionRequestDelete,
            bool maintenanceRequestView, bool maintenanceRequestAdd, bool maintenanceRequestEdit, bool maintenanceRequestDelete,
            bool demolitionRequestView, bool demolitionRequestAdd, bool demolitionRequestEdit, bool demolitionRequestDelete,
            bool nameChangeRequestView, bool nameChangeRequestAdd, bool nameChangeRequestEdit, bool nameChangeRequestDelete,
            bool needsRequestView, bool needsRequestAdd, bool needsRequestEdit, bool needsRequestDelete,
            bool expenditureChangeRequestView, bool expenditureChangeRequestAdd, bool expenditureChangeRequestEdit, bool expenditureChangeRequestDelete,
            bool changeOfPathRequestView, bool changeOfPathRequestAdd, bool changeOfPathRequestEdit, bool changeOfPathRequestDelete,
            bool officeView, bool officeAdd, bool officeEdit, bool officeDelete,
            bool endowmentView, bool endowmentAdd, bool endowmentEdit, bool endowmentDelete,
            bool cityView, bool cityAdd, bool cityEdit, bool cityDelete,
            bool regionView, bool regionAdd, bool regionEdit, bool regionDelete,
            bool buildingView, bool buildingAdd, bool buildingEdit, bool buildingDelete,
            bool mosqueView, bool mosqueAdd, bool mosqueEdit, bool mosqueDelete,
            bool productView, bool productAdd, bool productEdit, bool productDelete,
            bool bankView, bool bankAdd, bool bankEdit, bool bankDelete,
            bool branchView, bool branchAdd, bool branchEdit, bool branchDelete,
            bool facilityView, bool facilityAdd, bool facilityEdit, bool facilityDelete,
            bool buildingDetailRequestView, bool buildingDetailRequestAdd, bool buildingDetailRequestEdit, bool buildingDetailRequestDelete,
            bool quranicSchoolView, bool quranicSchoolAdd, bool quranicSchoolEdit, bool quranicSchoolDelete
            )
        {
            AccountView = accountView;
            AccountAdd = accountAdd;
            AccountEdit = accountEdit;
            AccountDelete = accountDelete;

            AccountDetailView = accountDetailView;
            AccountDetailAdd = accountDetailAdd;
            AccountDetailEdit = accountDetailEdit;
            AccountDetailDelete = accountDetailDelete;

            UserView = userView;
            UserAdd = userAdd;
            UserEdit = userEdit;
            UserDelete = userDelete;

            RoleView = roleView;
            RoleAdd = roleAdd;
            RoleEdit = roleEdit;
            RoleDelete = roleDelete;

            DecisionView = decisionView;
            DecisionAdd = decisionAdd;
            DecisionEdit = decisionEdit;
            DecisionDelete = decisionDelete;

            RequestView = requestView;
            RequestAdd = requestAdd;
            RequestEdit = requestEdit;
            RequestDelete = requestDelete;

            ConstructionRequestView = constructionRequestView;
            ConstructionRequestAdd = constructionRequestAdd;
            ConstructionRequestEdit = constructionRequestEdit;
            ConstructionRequestDelete = constructionRequestDelete;

            MaintenanceRequestView = maintenanceRequestView;
            MaintenanceRequestAdd = maintenanceRequestAdd;
            MaintenanceRequestEdit = maintenanceRequestEdit;
            MaintenanceRequestDelete = maintenanceRequestDelete;

            // removed duplicate capitalized parameters; use camelCase

            DemolitionRequestView = demolitionRequestView;
            DemolitionRequestAdd = demolitionRequestAdd;
            DemolitionRequestEdit = demolitionRequestEdit;
            DemolitionRequestDelete = demolitionRequestDelete;

            NameChangeRequestView = nameChangeRequestView;
            NameChangeRequestAdd = nameChangeRequestAdd;
            NameChangeRequestEdit = nameChangeRequestEdit;
            NameChangeRequestDelete = nameChangeRequestDelete;

            NeedsRequestView = needsRequestView;
            NeedsRequestAdd = needsRequestAdd;
            NeedsRequestEdit = needsRequestEdit;
            NeedsRequestDelete = needsRequestDelete;

            ChangeOfPathRequestView = changeOfPathRequestView;
            ChangeOfPathRequestAdd = changeOfPathRequestAdd;
            ChangeOfPathRequestEdit = changeOfPathRequestEdit;
            ChangeOfPathRequestDelete = changeOfPathRequestDelete;

            ExpenditureChangeRequestView = expenditureChangeRequestView;
            ExpenditureChangeRequestAdd = expenditureChangeRequestAdd;
            ExpenditureChangeRequestEdit = expenditureChangeRequestEdit;
            ExpenditureChangeRequestDelete = expenditureChangeRequestDelete;

            OfficeView = officeView;
            OfficeAdd = officeAdd;
            OfficeEdit = officeEdit;
            OfficeDelete = officeDelete;

            EndowmentView = endowmentView;
            EndowmentAdd = endowmentAdd;
            EndowmentEdit = endowmentEdit;
            EndowmentDelete = endowmentDelete;

            CityView = cityView;
            CityAdd = cityAdd;
            CityEdit = cityEdit;
            CityDelete = cityDelete;

            RegionView = regionView;
            RegionAdd = regionAdd;
            RegionEdit = regionEdit;
            RegionDelete = regionDelete;

            BuildingView = buildingView;
            BuildingAdd = buildingAdd;
            BuildingEdit = buildingEdit;
            BuildingDelete = buildingDelete;

            MosqueView = mosqueView;
            MosqueAdd = mosqueAdd;
            MosqueEdit = mosqueEdit;
            MosqueDelete = mosqueDelete;

            ProductView = productView;
            ProductAdd = productAdd;
            ProductEdit = productEdit;
            ProductDelete = productDelete;

            BankView = bankView;
            BankAdd = bankAdd;
            BankEdit = bankEdit;
            BankDelete = bankDelete;

            BranchView = branchView;
            BranchAdd = branchAdd;
            BranchEdit = branchEdit;
            BranchDelete = branchDelete;

            FacilityView = facilityView;
            FacilityAdd = facilityAdd;
            FacilityEdit = facilityEdit;
            FacilityDelete = facilityDelete;

            BuildingDetailRequestView = buildingDetailRequestView;
            BuildingDetailRequestAdd = buildingDetailRequestAdd;
            BuildingDetailRequestEdit = buildingDetailRequestEdit;
            BuildingDetailRequestDelete = buildingDetailRequestDelete;

            QuranicSchoolView = quranicSchoolView;
            QuranicSchoolAdd = quranicSchoolAdd;
            QuranicSchoolEdit = quranicSchoolEdit;
            QuranicSchoolDelete = quranicSchoolDelete;
        }

        public static Permission Create(
            bool accountView, bool accountAdd, bool accountEdit, bool accountDelete,
            bool accountDetailView, bool accountDetailAdd, bool accountDetailEdit, bool accountDetailDelete,
            bool userView, bool userAdd, bool userEdit, bool userDelete,
            bool roleView, bool roleAdd, bool roleEdit, bool roleDelete,
            bool decisionView, bool decisionAdd, bool decisionEdit, bool decisionDelete,
            bool requestView, bool requestAdd, bool requestEdit, bool requestDelete,
            bool constructionRequestView, bool constructionRequestAdd, bool constructionRequestEdit, bool constructionRequestDelete,
            bool maintenanceRequestView, bool maintenanceRequestAdd, bool maintenanceRequestEdit, bool maintenanceRequestDelete,
            // removed duplicate capitalized ChangeOfPathRequest parameters
            bool demolitionRequestView, bool demolitionRequestAdd, bool demolitionRequestEdit, bool demolitionRequestDelete,
            bool nameChangeRequestView, bool nameChangeRequestAdd, bool nameChangeRequestEdit, bool nameChangeRequestDelete,
            bool needsRequestView, bool needsRequestAdd, bool needsRequestEdit, bool needsRequestDelete,
            bool expenditureChangeRequestView, bool expenditureChangeRequestAdd, bool expenditureChangeRequestEdit, bool expenditureChangeRequestDelete,
            bool changeOfPathRequestView, bool changeOfPathRequestAdd, bool changeOfPathRequestEdit, bool changeOfPathRequestDelete,
            bool officeView, bool officeAdd, bool officeEdit, bool officeDelete,
            bool endowmentView, bool endowmentAdd, bool endowmentEdit, bool endowmentDelete,
            bool cityView, bool cityAdd, bool cityEdit, bool cityDelete,
            bool regionView, bool regionAdd, bool regionEdit, bool regionDelete,
            bool buildingView, bool buildingAdd, bool buildingEdit, bool buildingDelete,
            bool mosqueView, bool mosqueAdd, bool mosqueEdit, bool mosqueDelete,
            bool productView, bool productAdd, bool productEdit, bool productDelete,
            bool bankView, bool bankAdd, bool bankEdit, bool bankDelete,
            bool branchView, bool branchAdd, bool branchEdit, bool branchDelete,
            bool facilityView, bool facilityAdd, bool facilityEdit, bool facilityDelete,
            bool buildingDetailRequestView, bool buildingDetailRequestAdd, bool buildingDetailRequestEdit, bool buildingDetailRequestDelete,
            bool quranicSchoolView, bool quranicSchoolAdd, bool quranicSchoolEdit, bool quranicSchoolDelete)
        {
            return new Permission(
                accountView, accountAdd, accountEdit, accountDelete,
                accountDetailView, accountDetailAdd, accountDetailEdit, accountDetailDelete,
                userView, userAdd, userEdit, userDelete,
                roleView, roleAdd, roleEdit, roleDelete,
                decisionView, decisionAdd, decisionEdit, decisionDelete,
                requestView, requestAdd, requestEdit, requestDelete,
                constructionRequestView, constructionRequestAdd, constructionRequestEdit, constructionRequestDelete,
                maintenanceRequestView, maintenanceRequestAdd, maintenanceRequestEdit, maintenanceRequestDelete,
                // removed duplicate capitalized ChangeOfPathRequest parameters
                demolitionRequestView, demolitionRequestAdd, demolitionRequestEdit, demolitionRequestDelete,
                nameChangeRequestView, nameChangeRequestAdd, nameChangeRequestEdit, nameChangeRequestDelete,
                needsRequestView, needsRequestAdd, needsRequestEdit, needsRequestDelete,
                expenditureChangeRequestView, expenditureChangeRequestAdd, expenditureChangeRequestEdit, expenditureChangeRequestDelete,
                changeOfPathRequestView, changeOfPathRequestAdd, changeOfPathRequestEdit, changeOfPathRequestDelete,
                officeView, officeAdd, officeEdit, officeDelete,
                endowmentView, endowmentAdd, endowmentEdit, endowmentDelete,
                cityView, cityAdd, cityEdit, cityDelete,
                regionView, regionAdd, regionEdit, regionDelete,
                buildingView, buildingAdd, buildingEdit, buildingDelete,
                mosqueView, mosqueAdd, mosqueEdit, mosqueDelete,
                productView, productAdd, productEdit, productDelete,
                bankView, bankAdd, bankEdit, bankDelete,
                branchView, branchAdd, branchEdit, branchDelete,
                facilityView, facilityAdd, facilityEdit, facilityDelete,
                buildingDetailRequestView, buildingDetailRequestAdd, buildingDetailRequestEdit, buildingDetailRequestDelete,
                quranicSchoolView, quranicSchoolAdd, quranicSchoolEdit, quranicSchoolDelete);
        }

        public static Permission Seed()
        {
            return new Permission(
                accountView: true, accountAdd: true, accountEdit: true, accountDelete: true,
                accountDetailView: true, accountDetailAdd: true, accountDetailEdit: true, accountDetailDelete: true,
                userView: true, userAdd: true, userEdit: true, userDelete: true,
                roleView: true, roleAdd: true, roleEdit: true, roleDelete: true,
                decisionView: true, decisionAdd: true, decisionEdit: true, decisionDelete: true,
                requestView: true, requestAdd: true, requestEdit: true, requestDelete: true,
                constructionRequestView: true, constructionRequestAdd: true, constructionRequestEdit: true, constructionRequestDelete: true,
                maintenanceRequestView: true, maintenanceRequestAdd: true, maintenanceRequestEdit: true, maintenanceRequestDelete: true,
                changeOfPathRequestView: true, changeOfPathRequestAdd: true, changeOfPathRequestEdit: true, changeOfPathRequestDelete: true,
                demolitionRequestView: true, demolitionRequestAdd: true, demolitionRequestEdit: true, demolitionRequestDelete: true,
                nameChangeRequestView: true, nameChangeRequestAdd: true, nameChangeRequestEdit: true, nameChangeRequestDelete: true,
                needsRequestView: true, needsRequestAdd: true, needsRequestEdit: true, needsRequestDelete: true,
                expenditureChangeRequestView: true, expenditureChangeRequestAdd: true, expenditureChangeRequestEdit: true, expenditureChangeRequestDelete: true,
                officeView: true, officeAdd: true, officeEdit: true, officeDelete: true,
                endowmentView: true, endowmentAdd: true, endowmentEdit: true, endowmentDelete: true,
                cityView: true, cityAdd: true, cityEdit: true, cityDelete: true,
                regionView: true, regionAdd: true, regionEdit: true, regionDelete: true,
                buildingView: true, buildingAdd: true, buildingEdit: true, buildingDelete: true,
                mosqueView: true, mosqueAdd: true, mosqueEdit: true, mosqueDelete: true,
                productView: true, productAdd: true, productEdit: true, productDelete: true,
                bankView: true, bankAdd: true, bankEdit: true, bankDelete: true,
                branchView: true, branchAdd: true, branchEdit: true, branchDelete: true,
                facilityView: true, facilityAdd: true, facilityEdit: true, facilityDelete: true,
                buildingDetailRequestView: true, buildingDetailRequestAdd: true, buildingDetailRequestEdit: true, buildingDetailRequestDelete: true,
                quranicSchoolView: true, quranicSchoolAdd: true, quranicSchoolEdit: true, quranicSchoolDelete: true);
        }

        public List<string> ToPermissionList()
        {
            var permissions = new List<string>();

            if (AccountView) permissions.Add("Account_View");
            if (AccountAdd) permissions.Add("Account_Add");
            if (AccountEdit) permissions.Add("Account_Edit");
            if (AccountDelete) permissions.Add("Account_Delete");

            if (AccountDetailView) permissions.Add("AccountDetail_View");
            if (AccountDetailAdd) permissions.Add("AccountDetail_Add");
            if (AccountDetailEdit) permissions.Add("AccountDetail_Edit");
            if (AccountDetailDelete) permissions.Add("AccountDetail_Delete");

            if (UserView) permissions.Add("User_View");
            if (UserAdd) permissions.Add("User_Add");
            if (UserEdit) permissions.Add("User_Edit");
            if (UserDelete) permissions.Add("User_Delete");

            if (RoleView) permissions.Add("Role_View");
            if (RoleAdd) permissions.Add("Role_Add");
            if (RoleEdit) permissions.Add("Role_Edit");
            if (RoleDelete) permissions.Add("Role_Delete");

            if (DecisionView) permissions.Add("Decision_View");
            if (DecisionAdd) permissions.Add("Decision_Add");
            if (DecisionEdit) permissions.Add("Decision_Edit");
            if (DecisionDelete) permissions.Add("Decision_Delete");

            if (RequestView) permissions.Add("Request_View");
            if (RequestAdd) permissions.Add("Request_Add");
            if (RequestEdit) permissions.Add("Request_Edit");
            if (RequestDelete) permissions.Add("Request_Delete");

            if (ConstructionRequestView) permissions.Add("ConstructionRequest_View");
            if (ConstructionRequestAdd) permissions.Add("ConstructionRequest_Add");
            if (ConstructionRequestEdit) permissions.Add("ConstructionRequest_Edit");
            if (ConstructionRequestDelete) permissions.Add("ConstructionRequest_Delete");

            if (MaintenanceRequestView) permissions.Add("MaintenanceRequest_View");
            if (MaintenanceRequestAdd) permissions.Add("MaintenanceRequest_Add");
            if (MaintenanceRequestEdit) permissions.Add("MaintenanceRequest_Edit");
            if (MaintenanceRequestDelete) permissions.Add("MaintenanceRequest_Delete");

            // ChangeOfPathRequest unified naming

            if (DemolitionRequestView) permissions.Add("DemolitionRequest_View");
            if (DemolitionRequestAdd) permissions.Add("DemolitionRequest_Add");
            if (DemolitionRequestEdit) permissions.Add("DemolitionRequest_Edit");
            if (DemolitionRequestDelete) permissions.Add("DemolitionRequest_Delete");

            if (NameChangeRequestView) permissions.Add("NameChangeRequest_View");
            if (NameChangeRequestAdd) permissions.Add("NameChangeRequest_Add");
            if (NameChangeRequestEdit) permissions.Add("NameChangeRequest_Edit");
            if (NameChangeRequestDelete) permissions.Add("NameChangeRequest_Delete");

            if (NeedsRequestView) permissions.Add("NeedsRequest_View");
            if (NeedsRequestAdd) permissions.Add("NeedsRequest_Add");
            if (NeedsRequestEdit) permissions.Add("NeedsRequest_Edit");
            if (NeedsRequestDelete) permissions.Add("NeedsRequest_Delete");

            if (ExpenditureChangeRequestView) permissions.Add("ExpenditureChangeRequest_View");
            if (ExpenditureChangeRequestAdd) permissions.Add("ExpenditureChangeRequest_Add");
            if (ExpenditureChangeRequestEdit) permissions.Add("ExpenditureChangeRequest_Edit");
            if (ExpenditureChangeRequestDelete) permissions.Add("ExpenditureChangeRequest_Delete");

            if (ChangeOfPathRequestView) permissions.Add("ChangeOfPathRequest_View");
            if (ChangeOfPathRequestAdd) permissions.Add("ChangeOfPathRequest_Add");
            if (ChangeOfPathRequestEdit) permissions.Add("ChangeOfPathRequest_Edit");
            if (ChangeOfPathRequestDelete) permissions.Add("ChangeOfPathRequest_Delete");

            if (OfficeView) permissions.Add("Office_View");
            if (OfficeAdd) permissions.Add("Office_Add");
            if (OfficeEdit) permissions.Add("Office_Edit");
            if (OfficeDelete) permissions.Add("Office_Delete");

            if (EndowmentView) permissions.Add("Endowment_View");
            if (EndowmentAdd) permissions.Add("Endowment_Add");
            if (EndowmentEdit) permissions.Add("Endowment_Edit");
            if (EndowmentDelete) permissions.Add("Endowment_Delete");

            if (CityView) permissions.Add("City_View");
            if (CityAdd) permissions.Add("City_Add");
            if (CityEdit) permissions.Add("City_Edit");
            if (CityDelete) permissions.Add("City_Delete");

            if (RegionView) permissions.Add("Region_View");
            if (RegionAdd) permissions.Add("Region_Add");
            if (RegionEdit) permissions.Add("Region_Edit");
            if (RegionDelete) permissions.Add("Region_Delete");

            if (BuildingView) permissions.Add("Building_View");
            if (BuildingAdd) permissions.Add("Building_Add");
            if (BuildingEdit) permissions.Add("Building_Edit");
            if (BuildingDelete) permissions.Add("Building_Delete");

            if (MosqueView) permissions.Add("Mosque_View");
            if (MosqueAdd) permissions.Add("Mosque_Add");
            if (MosqueEdit) permissions.Add("Mosque_Edit");
            if (MosqueDelete) permissions.Add("Mosque_Delete");

            if (ProductView) permissions.Add("Product_View");
            if (ProductAdd) permissions.Add("Product_Add");
            if (ProductEdit) permissions.Add("Product_Edit");
            if (ProductDelete) permissions.Add("Product_Delete");

            if (BankView) permissions.Add("Bank_View");
            if (BankAdd) permissions.Add("Bank_Add");
            if (BankEdit) permissions.Add("Bank_Edit");
            if (BankDelete) permissions.Add("Bank_Delete");

            if (BranchView) permissions.Add("Branch_View");
            if (BranchAdd) permissions.Add("Branch_Add");
            if (BranchEdit) permissions.Add("Branch_Edit");
            if (BranchDelete) permissions.Add("Branch_Delete");

            if (FacilityView) permissions.Add("Facility_View");
            if (FacilityAdd) permissions.Add("Facility_Add");
            if (FacilityEdit) permissions.Add("Facility_Edit");
            if (FacilityDelete) permissions.Add("Facility_Delete");

            if (BuildingDetailRequestView) permissions.Add("BuildingDetailRequest_View");
            if (BuildingDetailRequestAdd) permissions.Add("BuildingDetailRequest_Add");
            if (BuildingDetailRequestEdit) permissions.Add("BuildingDetailRequest_Edit");
            if (BuildingDetailRequestDelete) permissions.Add("BuildingDetailRequest_Delete");

            if (QuranicSchoolView) permissions.Add("QuranicSchool_View");
            if (QuranicSchoolAdd) permissions.Add("QuranicSchool_Add");
            if (QuranicSchoolEdit) permissions.Add("QuranicSchool_Edit");
            if (QuranicSchoolDelete) permissions.Add("QuranicSchool_Delete");

            // إضافة logging لمعرفة الصلاحيات التي تم إضافتها
            Console.WriteLine($"🔍 ToPermissionList - ConstructionRequest permissions:");
            Console.WriteLine($"  - ConstructionRequestView: {ConstructionRequestView}");
            Console.WriteLine($"  - ConstructionRequestAdd: {ConstructionRequestAdd}");
            Console.WriteLine($"  - ConstructionRequestEdit: {ConstructionRequestEdit}");
            Console.WriteLine($"  - ConstructionRequestDelete: {ConstructionRequestDelete}");

            return permissions;
        }
    }
}

