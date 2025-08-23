using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Mosques.Commands.Delete
{
    [Authorize(Permissions = Permission.MosqueDelete)]
    public class DeleteMosqueCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}