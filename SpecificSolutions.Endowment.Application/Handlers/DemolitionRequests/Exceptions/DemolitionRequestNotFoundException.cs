using System;

namespace SpecificSolutions.Endowment.Application.Handlers.DemolitionRequests.Exceptions
{
    public class DemolitionRequestNotFoundException : Exception
    {
        public DemolitionRequestNotFoundException() : base("Demolition and Reconstruction Request not found.") { }
    }
} 