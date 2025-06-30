using SpecificSolutions.Endowment.Application.Abstractions.Messaging;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Delete
{
    public class DeleteQuranicSchoolCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}