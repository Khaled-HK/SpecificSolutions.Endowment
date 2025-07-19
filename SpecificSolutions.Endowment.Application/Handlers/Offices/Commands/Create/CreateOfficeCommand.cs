using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Core.Models.Office;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Create
{
    public class CreateOfficeCommand : ICommand, ICreateOfficeCommand
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public Guid RegionId { get; set; }
    }
}