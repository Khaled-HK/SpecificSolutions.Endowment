using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.Offices.Commands.Delete
{
    public class DeleteOfficeCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}