namespace SpecificSolutions.Endowment.Application.Models.Identity
{
    public sealed class Permission
    {
        public bool AccountView { get; private set; }
        public bool AccountAdd { get; private set; }
        public bool AccountEdit { get; private set; }
        public bool AccountDelete { get; private set; }

        public bool UserView { get; private set; }
        public bool UserAdd { get; private set; }
        public bool UserEdit { get; private set; }
        public bool UserDelete { get; private set; }

        public bool RoleView { get; private set; }
        public bool RoleAdd { get; private set; }
        public bool RoleEdit { get; private set; }
        public bool RoleDelete { get; private set; }

        public bool DecisionView { get; private set; }
        public bool DecisionAdd { get; private set; }
        public bool DecisionEdit { get; private set; }
        public bool DecisionDelete { get; private set; }

        public bool RequestView { get; private set; }
        public bool RequestAdd { get; private set; }
        public bool RequestEdit { get; private set; }
        public bool RequestDelete { get; private set; }

        public bool OfficeView { get; private set; }
        public bool OfficeAdd { get; private set; }
        public bool OfficeEdit { get; private set; }
        public bool OfficeDelete { get; private set; }

        private Permission() { }

        public static Permission Create(
            bool accountView, bool accountAdd, bool accountEdit, bool accountDelete,
            bool userView, bool userAdd, bool userEdit, bool userDelete,
            bool roleView, bool roleAdd, bool roleEdit, bool roleDelete,
            bool decisionView, bool decisionAdd, bool decisionEdit, bool decisionDelete,
            bool requestView, bool requestAdd, bool requestEdit, bool requestDelete,
            bool officeView, bool officeAdd, bool officeEdit, bool officeDelete)
        {
            return new Permission
            {
                AccountView = accountView,
                AccountAdd = accountAdd,
                AccountEdit = accountEdit,
                AccountDelete = accountDelete,

                UserView = userView,
                UserAdd = userAdd,
                UserEdit = userEdit,
                UserDelete = userDelete,

                RoleView = roleView,
                RoleAdd = roleAdd,
                RoleEdit = roleEdit,
                RoleDelete = roleDelete,

                DecisionView = decisionView,
                DecisionAdd = decisionAdd,
                DecisionEdit = decisionEdit,
                DecisionDelete = decisionDelete,

                RequestView = requestView,
                RequestAdd = requestAdd,
                RequestEdit = requestEdit,
                RequestDelete = requestDelete,

                OfficeView = officeView,
                OfficeAdd = officeAdd,
                OfficeEdit = officeEdit,
                OfficeDelete = officeDelete
            };
        }

        public static Permission Seed()
        {
            return new Permission
            {
                AccountView = true,
                AccountAdd = true,
                AccountEdit = true,
                AccountDelete = true,

                UserView = true,
                UserAdd = true,
                UserEdit = true,
                UserDelete = true,

                RoleView = true,
                RoleAdd = true,
                RoleEdit = true,
                RoleDelete = true,

                DecisionView = true,
                DecisionAdd = true,
                DecisionEdit = true,
                DecisionDelete = true,

                RequestView = true,
                RequestAdd = true,
                RequestEdit = true,
                RequestDelete = true,

                OfficeView = true,
                OfficeAdd = true,
                OfficeEdit = true,
                OfficeDelete = true
            };
        }
    }
}
