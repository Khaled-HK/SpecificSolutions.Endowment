using SpecificSolutions.Endowment.Core.Enums.Mosques;
using SpecificSolutions.Endowment.Core.Models.Buildings;

namespace SpecificSolutions.Endowment.Core.Models.Mosques
{
    public interface ICreateMosqueCommand : ICreateBuildingCommand
    {
        MosqueDefinition MosqueDefinition { get; }
        MosqueClassification MosqueClassification { get; }
    }
}
