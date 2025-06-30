using System;

namespace SpecificSolutions.Endowment.Application.Handlers.NameChangeRequests.Exceptions
{
    public class NameChangeRequestNotFoundException : Exception
    {
        public NameChangeRequestNotFoundException() : base("Name Change Request not found.") { }
    }
} 