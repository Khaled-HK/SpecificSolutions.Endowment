using SpecificSolutions.Endowment.Core.Entities.Buildings;
using SpecificSolutions.Endowment.Core.Enums.Mosques;
using SpecificSolutions.Endowment.Core.Models.Mosques;

namespace SpecificSolutions.Endowment.Core.Entities.Mosques
{
    public class Mosque
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid BuildingId { get; private set; }
        public Building Building { get; private set; }

        public MosqueDefinition MosqueDefinition { get; private set; }
        public MosqueClassification MosqueClassification { get; private set; }

        private Mosque() { }

        public static Mosque Create(ICreateMosqueCommand command, Building building)
        {
            return new Mosque
            {
                BuildingId = building.Id,
                Building = building,
                MosqueDefinition = command.MosqueDefinition,
                MosqueClassification = command.MosqueClassification,
            };
        }

        public void Update(IUpdateMosqueCommand command)
        {
            MosqueDefinition = command.MosqueDefinition;
            MosqueClassification = command.MosqueClassification;
        }
    }
}