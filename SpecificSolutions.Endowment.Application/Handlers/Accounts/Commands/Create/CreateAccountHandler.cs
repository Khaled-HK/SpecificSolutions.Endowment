using SpecificSolutions.Endowment.Application.Abstractions.IRepositories;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using SpecificSolutions.Endowment.Application.Models.Global;
using SpecificSolutions.Endowment.Core.Entities.Accounts;
using SpecificSolutions.Endowment.Core.Entities.AuditLogs;
using SpecificSolutions.Endowment.Core.Enums;
using SpecificSolutions.Endowment.Core.Helpers;

namespace SpecificSolutions.Endowment.Application.Handlers.Accounts.Commands.Create
{
    public class CreateAccountHandler : ICommandHandler<CreateAccountCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public CreateAccountHandler(
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<EndowmentResponse> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
        {
            // Validate input (you can also use FluentValidation for this)
            if (string.IsNullOrEmpty(command.Name))
            {
                return Response.FailureResponse("Name", "Name is required.");
            }

            if (string.IsNullOrEmpty(command.MotherName))
            {
                return Response.FailureResponse("MotherName", "Mother's name is required.");
            }

            // Create a new Account entity
            var account = Account.Create(command, _currentUser.Id);
            var auditLog = AuditLog.Create(_currentUser.Id, EntityContext.Account, Helper.Serialize(command));

            // Add the account to the repository
            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.AuditLogs.AddAsync(auditLog);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return Response.Added();
        }
    }
}