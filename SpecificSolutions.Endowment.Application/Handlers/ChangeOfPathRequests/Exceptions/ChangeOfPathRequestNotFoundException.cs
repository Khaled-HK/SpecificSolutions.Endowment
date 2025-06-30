using System;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Exceptions
{
    public class ChangeOfPathRequestNotFoundException : Exception
    {
        public ChangeOfPathRequestNotFoundException() : base("Change of Path Request not found.") { }
    }
} 