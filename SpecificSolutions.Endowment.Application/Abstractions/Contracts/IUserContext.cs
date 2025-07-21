namespace SpecificSolutions.Endowment.Application.Abstractions.Contracts
{
    /// <summary>
    /// Provides access to the current user context
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// Gets the current user ID. Throws exception if not available.
        /// </summary>
        /// <exception cref="ApplicationException">Thrown when user context is unavailable</exception>
        Guid UserId { get; }

        /// <summary>
        /// Gets the current user ID if available, returns null if not available
        /// </summary>
        /// <returns>User ID if available, null otherwise</returns>
        Guid? GetUserIdOrDefault();

        /// <summary>
        /// Checks if UserId is available
        /// </summary>
        /// <returns>True if UserId is available, false otherwise</returns>
        bool HasUserId();

        /// <summary>
        /// Sets the current user ID
        /// </summary>
        /// <param name="userId">The user ID to set</param>
        void SetUserId(string userId);
    }
}
