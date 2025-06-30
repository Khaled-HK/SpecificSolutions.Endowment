using System;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Exceptions
{
    public class MosqueNotFoundException : Exception
    {
        public MosqueNotFoundException() : base("Mosque not found.") { }
    }
} 