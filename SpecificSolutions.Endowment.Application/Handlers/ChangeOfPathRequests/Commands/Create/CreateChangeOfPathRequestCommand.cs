using SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create;

namespace SpecificSolutions.Endowment.Application.Handlers.ChangeOfPathRequests.Commands.Create
{
    public class CreateChangeOfPathRequestCommand : CreateRequestCommand
    {
        public string CurrentType { get; set; } = string.Empty;
        public string NewType { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}