using System;

namespace SpecificSolutions.Endowment.Application.Handlers.Facilities.Exceptions
{
    public class FacilityNotFoundException : Exception
    {
        public FacilityNotFoundException() : base("Facility not found.") { }
    }
} 