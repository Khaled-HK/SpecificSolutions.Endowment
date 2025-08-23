using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.QuranicSchools.Commands.Delete
{
    [Authorize(Permissions = Permission.QuranicSchoolDelete)]
    public class DeleteQuranicSchoolCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}