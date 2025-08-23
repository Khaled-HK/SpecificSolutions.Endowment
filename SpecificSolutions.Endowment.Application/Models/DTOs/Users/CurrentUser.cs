using SpecificSolutions.Endowment.Core.Enums;

namespace SpecificSolutions.Endowment.Application.Models.DTOs.Users;
//Todo refactor and implement with defrence ath
public interface ICurrentUser
{
    // User ID Properties
    string Id { get; set; }
    Guid UserId { get; }
    Guid? GetUserIdOrDefault();
    bool HasUserId();
    void SetUserId(string userId);

    // User Information Properties
    string UserName { get; set; }

    // Authorization Methods
    Task<bool> HasPermissionAsync(Permission permission);

    // User Management Methods
    void UpdateUserInfo(IUserLogin user);
}

public class CurrentUser : ICurrentUser
{
    private readonly Dictionary<string, (Guid Id, string Name, List<string> Permissions, DateTime LoginAt)> _currentUser = new();
    private Guid? _userId;

    public virtual string Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Permission { get; set; } = string.Empty;

    // User ID Implementation
    public Guid UserId => _userId ?? throw new ApplicationException("User context is unavailable - No UserId found");
    public Guid? GetUserIdOrDefault() => _userId;
    public bool HasUserId() => _userId.HasValue && _userId.Value != Guid.Empty;

    public void SetUserId(string userId)
    {
        Id = userId;
        if (Guid.TryParse(userId, out var guid))
            _userId = guid;
    }

    public void UpdateUserInfo(IUserLogin user)
    {
        Id = user.Id;
        UserName = user.Name;
        SetUserId(user.Id);
        //Permission = user.Permissions
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