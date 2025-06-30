using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Models.Identity;

namespace SpecificSolutions.Endowment.Application.Handlers.Authentications.Commands.RefreshToken
{
    public record RefreshTokenCommand(/*string Token,*/ string RefreshToken) : ICommand<RefreshTokenResponse>;
}
