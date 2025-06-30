using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.Users;
//Todo refactor and implement with defrence ath
public interface ICurrentUser
{
    public string Id { get; set; }
    string UserName { get; }
    Task<bool> HasPermissionAsync(Permission permission);
    void UpdateUserInfo(IUserLogin user);
}

public class CurrentUser : ICurrentUser
{
    private readonly Dictionary<string, (Guid Id, string Name, List<string> Permissions, DateTime LoginAt)> _currentUser = new();

    public virtual string Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Permission { get; set; } = string.Empty;

    public void UpdateUserInfo(IUserLogin user)
    {
        Id = user.Id;
        UserName = user.Name;
    }

    public virtual async Task<bool> HasPermissionAsync(Permission permission)
    {
        //Todo refactor and implement with defrence ath id .ToString() if needed when null

        if (string.IsNullOrEmpty(Id.ToString()))
        {
            throw new ArgumentNullException(nameof(Id), "User ID cannot be null or empty.");
        }

        if (_currentUser.TryGetValue(Id.ToString(), out var currentUser))
        {
            return await Task.FromResult(currentUser.Permissions.Contains(permission.ToString()));
        }

        return await Task.FromResult(false);
    }
}