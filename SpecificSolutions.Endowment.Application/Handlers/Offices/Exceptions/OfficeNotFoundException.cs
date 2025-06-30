using System;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Exceptions
{
    public class OfficeNotFoundException : Exception
    {
        public OfficeNotFoundException(Guid id) 
            : base($"Office with ID {id} not found.")
        {
        }
    }
} 