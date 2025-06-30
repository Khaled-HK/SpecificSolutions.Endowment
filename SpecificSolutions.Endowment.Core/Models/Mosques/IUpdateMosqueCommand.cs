
using SpecificSolutions.Endowment.Core.Enums.Mosques;
using SpecificSolutions.Endowment.Core.Models.Buildings;

namespace SpecificSolutions.Endowment.Core.Models.Mosques
{
    public interface IUpdateMosqueCommand : IUpdateBuildingCommand
    {
        public MosqueDefinition MosqueDefinition { get; set; }
        public MosqueClassification MosqueClassification { get; set; }
    }
}
