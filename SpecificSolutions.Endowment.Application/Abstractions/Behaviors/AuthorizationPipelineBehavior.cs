using MediatR;
using SpecificSolutions.Endowment.Application.Abstractions.Messaging;
using SpecificSolutions.Endowment.Application.Abstractions.Requests;
using SpecificSolutions.Endowment.Application.Models.DTOs.Users;
using System.Reflection;
//TODO refactor
public class AuthorizationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
    where TResponse : notnull//EndowmentResponse, new()
{
    // private current user
    private readonly ICurrentUser _currentUser;
    public AuthorizationPipelineBehavior(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizationAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        var requiredPermissions = authorizationAttributes.FirstOrDefault();

        if (requiredPermissions == null)
        {
            return await next();
        }

        if (!await _currentUser.HasPermissionAsync(requiredPermissions.Permissions))
        {
            throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        }

        //if (request is IAuthenticator authenticator)
        //{
        //    if (!await _authorizationService.HasPermissionAsync(authenticator.Permission))
        //    {
        //        throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        //    }
        //}

        // Check if the user is authorized to execute the request
        //if (!await _authorizationService.IsUserInRoleAsync("Admin"))
        //{
        //    throw new UnauthorizedAccessException("You do not have permission to perform this action.");
        //}

        // Proceed to the next behavior or handler
        return await next();
    }
}