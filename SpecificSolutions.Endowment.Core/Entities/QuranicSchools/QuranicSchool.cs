using SpecificSolutions.Endowment.Core.Entities.Buildings;

namespace SpecificSolutions.Endowment.Core.Entities.QuranicSchools
{
    public class QuranicSchool
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Building Building { get; private set; } // One-to-one relationship

        private QuranicSchool() { }

        public static QuranicSchool Create(Building building)
        {
            return new QuranicSchool
            {
                Building = building,
            };
        }
    }
}