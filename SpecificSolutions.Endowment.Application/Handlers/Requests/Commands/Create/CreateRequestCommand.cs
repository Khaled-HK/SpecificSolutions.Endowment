using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Handlers.Requests.Commands.Create
{
    [Authorize(Permissions = Permission.RequestAdd)]
    public class CreateRequestCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Priority { get; set; }
        public string Location { get; set; }
        public string ReferenceNumber { get; set; }
        public List<string> Attachments { get; set; }
        public string RequestStatus { get; set; }
        public string Description { get; set; }
        public Guid DecisionId { get; internal set; }
    }
}