using System.Collections.Concurrent;
using TicTacToe.Web.Models;

namespace TicTacToe.Web.Services;

/// <summary>
/// Manages connected users and lobby functionality.
/// </summary>
public class LobbyService
{
    private readonly ConcurrentDictionary<string, ConnectedUser> _connectedUsers = new();

    /// <summary>
    /// Adds a user to the lobby.
    /// </summary>
    /// <param name="connectionId">The user's connection ID.</param>
    /// <param name="username">The user's username.</param>
    /// <returns>True if added successfully, false if user already exists.</returns>
    public bool AddUser(string connectionId, string username)
    {
        var user = new ConnectedUser
        {
            ConnectionId = connectionId,
            Username = username,
            Status = UserStatus.Available,
            ConnectedAt = DateTime.UtcNow
        };

        return _connectedUsers.TryAdd(connectionId, user);
    }

    /// <summary>
    /// Removes a user from the lobby.
    /// </summary>
    /// <param name="connectionId">The user's connection ID.</param>
    /// <returns>True if removed successfully.</returns>
    public bool RemoveUser(string connectionId)
    {
        return _connectedUsers.TryRemove(connectionId, out _);
    }

    /// <summary>
    /// Gets a user by connection ID.
    /// </summary>
    /// <param name="connectionId">The user's connection ID.</param>
    /// <returns>The connected user or null if not found.</returns>
    public ConnectedUser? GetUser(string connectionId)
    {
        _connectedUsers.TryGetValue(connectionId, out var user);
        return user;
    }

    /// <summary>
    /// Gets all connected users.
    /// </summary>
    /// <returns>A list of all connected users.</returns>
    public List<ConnectedUser> GetAllUsers()
    {
        return _connectedUsers.Values.ToList();
    }

    /// <summary>
    /// Gets all available users (not in game).
    /// </summary>
    /// <returns>A list of available users.</returns>
    public List<ConnectedUser> GetAvailableUsers()
    {
        return _connectedUsers.Values
            .Where(u => u.Status == UserStatus.Available)
            .ToList();
    }

    /// <summary>
    /// Updates a user's status.
    /// </summary>
    /// <param name="connectionId">The user's connection ID.</param>
    /// <param name="status">The new status.</param>
    /// <param name="gameId">The game ID if applicable.</param>
    /// <returns>True if updated successfully.</returns>
    public bool UpdateUserStatus(string connectionId, UserStatus status, string? gameId = null)
    {
        if (_connectedUsers.TryGetValue(connectionId, out var user))
        {
            user.Status = status;
            user.CurrentGameId = gameId;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if a username is already taken.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <returns>True if the username is taken.</returns>
    public bool IsUsernameTaken(string username)
    {
        return _connectedUsers.Values.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }
}
