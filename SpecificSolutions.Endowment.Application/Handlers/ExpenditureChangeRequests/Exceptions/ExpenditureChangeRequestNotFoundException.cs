using System;

namespace SpecificSolutions.Endowment.Application.Handlers.EndowmentExpenditureChangeRequests.Exceptions
{
    public class ExpenditureChangeRequestNotFoundException : Exception
    {
        public ExpenditureChangeRequestNotFoundException() : base("Endowment Expenditure Change Request not found.") { }
    }
} 