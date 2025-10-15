namespace TicTacToe.Web.Models;

/// <summary>
/// Represents a connected user in the lobby.
/// </summary>
public class ConnectedUser
{
    /// <summary>
    /// Gets or sets the unique connection ID from SignalR.
    /// </summary>
    public string ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user status.
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Available;

    /// <summary>
    /// Gets or sets the current game ID if in a game.
    /// </summary>
    public string? CurrentGameId { get; set; }

    /// <summary>
    /// Gets or sets when the user connected.
    /// </summary>
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// User status enumeration.
/// </summary>
public enum UserStatus
{
    Available,
    InGame,
    PendingInvitation
}
