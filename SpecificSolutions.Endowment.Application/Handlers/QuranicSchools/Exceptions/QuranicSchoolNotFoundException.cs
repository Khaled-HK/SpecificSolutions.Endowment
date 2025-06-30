using System;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Exceptions
{
    public class QuranicSchoolNotFoundException : Exception
    {
        public QuranicSchoolNotFoundException() : base("Quranic School not found.") { }
    }
} 