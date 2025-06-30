using System;

namespace SpecificSolutions.Endowment.Application.Handlers.MaintenanceRequests.Exceptions
{
    public class MaintenanceRequestNotFoundException : Exception
    {
        public MaintenanceRequestNotFoundException() : base("Maintenance Request not found.") { }
    }
} 