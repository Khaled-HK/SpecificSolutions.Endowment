using System;

namespace SpecificSolutions.Endowment.Application.Handlers.ConstructionRequests.Exceptions
{
    public class ConstructionRequestNotFoundException : Exception
    {
        public ConstructionRequestNotFoundException() : base("Construction Request not found.") { }
    }
} 