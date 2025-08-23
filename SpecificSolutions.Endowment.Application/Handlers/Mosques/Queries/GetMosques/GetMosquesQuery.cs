using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Global;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Queries.GetMosques
{
    /// <summary>
    /// Query لجلب قائمة المساجد - نمط خالد
    /// </summary>
    public record GetMosquesQuery : IQuery<IEnumerable<KeyValuPair>>
    {
        // خصائص مطلوبة من IQuery حتى لو لم تُستخدم هنا
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
} 