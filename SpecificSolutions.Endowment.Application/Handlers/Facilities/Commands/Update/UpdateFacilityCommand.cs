using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Facilities.Commands.Update
{
    public class UpdateFacilityCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ContactInfo { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
    }
}