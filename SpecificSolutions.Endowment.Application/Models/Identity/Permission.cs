using System.Text.Json.Serialization;

namespace SpecificSolutions.Endowment.Application.Models.Identity
{
    public sealed class Permission
    {
        public bool AccountView { get; set; }
        public bool AccountAdd { get; set; }
        public bool AccountEdit { get; set; }
        public bool AccountDelete { get; set; }

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

        // Public parameterless constructor for JSON deserialization
        public Permission() { }

        // JsonConstructor for parameterized constructor
        [JsonConstructor]
        public Permission(
            bool accountView, bool accountAdd, bool accountEdit, bool accountDelete,
            bool userView, bool userAdd, bool userEdit, bool userDelete,
            bool roleView, bool roleAdd, bool roleEdit, bool roleDelete,
            bool decisionView, bool decisionAdd, bool decisionEdit, bool decisionDelete,
            bool requestView, bool requestAdd, bool requestEdit, bool requestDelete,
            bool officeView, bool officeAdd, bool officeEdit, bool officeDelete,
            bool endowmentView, bool endowmentAdd, bool endowmentEdit, bool endowmentDelete,
            bool cityView, bool cityAdd, bool cityEdit, bool cityDelete,
            bool regionView, bool regionAdd, bool regionEdit, bool regionDelete,
            bool buildingView, bool buildingAdd, bool buildingEdit, bool buildingDelete
            )
        {
            AccountView = accountView;
            AccountAdd = accountAdd;
            AccountEdit = accountEdit;
            AccountDelete = accountDelete;

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
        }

        public static Permission Create(
            bool accountView, bool accountAdd, bool accountEdit, bool accountDelete,
            bool userView, bool userAdd, bool userEdit, bool userDelete,
            bool roleView, bool roleAdd, bool roleEdit, bool roleDelete,
            bool decisionView, bool decisionAdd, bool decisionEdit, bool decisionDelete,
            bool requestView, bool requestAdd, bool requestEdit, bool requestDelete,
            bool officeView, bool officeAdd, bool officeEdit, bool officeDelete,
            bool endowmentView, bool endowmentAdd, bool endowmentEdit, bool endowmentDelete,
            bool cityView, bool cityAdd, bool cityEdit, bool cityDelete,
            bool regionView, bool regionAdd, bool regionEdit, bool regionDelete,
            bool buildingView, bool buildingAdd, bool buildingEdit, bool buildingDelete)
        {
            return new Permission(
                accountView, accountAdd, accountEdit, accountDelete,
                userView, userAdd, userEdit, userDelete,
                roleView, roleAdd, roleEdit, roleDelete,
                decisionView, decisionAdd, decisionEdit, decisionDelete,
                requestView, requestAdd, requestEdit, requestDelete,
                officeView, officeAdd, officeEdit, officeDelete,
                endowmentView, endowmentAdd, endowmentEdit, endowmentDelete,
                cityView, cityAdd, cityEdit, cityDelete,
                regionView, regionAdd, regionEdit, regionDelete,
                buildingView, buildingAdd, buildingEdit, buildingDelete);
        }

        public static Permission Seed()
        {
            return new Permission(
                accountView: true, accountAdd: true, accountEdit: true, accountDelete: true,
                userView: true, userAdd: true, userEdit: true, userDelete: true,
                roleView: true, roleAdd: true, roleEdit: true, roleDelete: true,
                decisionView: true, decisionAdd: true, decisionEdit: true, decisionDelete: true,
                requestView: true, requestAdd: true, requestEdit: true, requestDelete: true,
                officeView: true, officeAdd: true, officeEdit: true, officeDelete: true,
                endowmentView: true, endowmentAdd: true, endowmentEdit: true, endowmentDelete: true,
                cityView: true, cityAdd: true, cityEdit: true, cityDelete: true,
                regionView: true, regionAdd: true, regionEdit: true, regionDelete: true,
                buildingView: true, buildingAdd: true, buildingEdit: true, buildingDelete: true);
        }

        public List<string> ToPermissionList()
        {
            var permissions = new List<string>();
            
            if (AccountView) permissions.Add("Account_View");
            if (AccountAdd) permissions.Add("Account_Add");
            if (AccountEdit) permissions.Add("Account_Edit");
            if (AccountDelete) permissions.Add("Account_Delete");
            
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
            
            return permissions;
        }
    }
}
