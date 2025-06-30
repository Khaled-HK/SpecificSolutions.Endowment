using System;

namespace SpecificSolutions.Endowment.Application.Handlers.NeedsRequests.Exceptions
{
    public class NeedsRequestNotFoundException : Exception
    {
        public NeedsRequestNotFoundException() : base("Needs Request not found.") { }
    }
} 