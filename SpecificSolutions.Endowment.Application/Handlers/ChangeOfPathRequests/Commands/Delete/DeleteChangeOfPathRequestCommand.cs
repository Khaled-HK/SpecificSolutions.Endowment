using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Delete
{
    [Authorize(Permissions = Permission.ChangeOfPathRequestDelete)]
    public class DeleteChangeOfPathRequestCommand : ICommand
    {
        public Guid ChangeOfPathRequestID { get; set; }
    }
}