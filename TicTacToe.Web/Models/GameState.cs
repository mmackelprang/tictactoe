namespace TicTacToe.Web.Models;

/// <summary>
/// Represents the serializable game state for synchronization.
/// </summary>
public class GameState
{
    /// <summary>
    /// Gets or sets the game ID.
    /// </summary>
    public string GameId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the board size.
    /// </summary>
    public int BoardSize { get; set; } = 3;

    /// <summary>
    /// Gets or sets the win condition.
    /// </summary>
    public int WinCondition { get; set; } = 3;

    /// <summary>
    /// Gets or sets whether this is a 3D game.
    /// </summary>
    public bool Is3D { get; set; }

    /// <summary>
    /// Gets or sets the number of layers (depth) in the board.
    /// </summary>
    public int Layers { get; set; } = 1;

    /// <summary>
    /// Gets or sets the current board state (flattened 3D array: layer * size * size + row * size + col).
    /// </summary>
    public char[] BoardData { get; set; } = Array.Empty<char>();

    /// <summary>
    /// Gets or sets the player 1 connection ID.
    /// </summary>
    public string Player1ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player 1 username.
    /// </summary>
    public string Player1Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player 1 mark.
    /// </summary>
    public char Player1Mark { get; set; } = 'X';

    /// <summary>
    /// Gets or sets the player 2 connection ID (empty for AI games).
    /// </summary>
    public string Player2ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player 2 username.
    /// </summary>
    public string Player2Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player 2 mark.
    /// </summary>
    public char Player2Mark { get; set; } = 'O';

    /// <summary>
    /// Gets or sets the current player's connection ID.
    /// </summary>
    public string CurrentPlayerConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the game is against AI.
    /// </summary>
    public bool IsAIGame { get; set; }

    /// <summary>
    /// Gets or sets whether the game is finished.
    /// </summary>
    public bool IsGameOver { get; set; }

    /// <summary>
    /// Gets or sets the winner connection ID (empty for draw).
    /// </summary>
    public string? WinnerConnectionId { get; set; }

    /// <summary>
    /// Gets or sets the game result message.
    /// </summary>
    public string? ResultMessage { get; set; }
}
