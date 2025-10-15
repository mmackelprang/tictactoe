using TicTacToe;

namespace TicTacToe.Web.Models;

/// <summary>
/// Represents an active game session.
/// </summary>
public class GameSession
{
    /// <summary>
    /// Gets or sets the unique game ID.
    /// </summary>
    public string GameId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the game board.
    /// </summary>
    public Board Board { get; set; }

    /// <summary>
    /// Gets or sets player 1.
    /// </summary>
    public Player Player1 { get; set; }

    /// <summary>
    /// Gets or sets player 2.
    /// </summary>
    public Player Player2 { get; set; }

    /// <summary>
    /// Gets or sets the current player.
    /// </summary>
    public Player CurrentPlayer { get; set; }

    /// <summary>
    /// Gets or sets player 1's connection ID.
    /// </summary>
    public string Player1ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets player 2's connection ID (empty for AI games).
    /// </summary>
    public string Player2ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this is an AI game.
    /// </summary>
    public bool IsAIGame { get; set; }

    /// <summary>
    /// Gets or sets whether the game is over.
    /// </summary>
    public bool IsGameOver { get; set; }

    /// <summary>
    /// Gets or sets the winner (null for draw).
    /// </summary>
    public Player? Winner { get; set; }

    /// <summary>
    /// Gets or sets when the game was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Initializes a new instance of the GameSession class.
    /// </summary>
    public GameSession(Board board, Player player1, Player player2, string player1ConnectionId, string player2ConnectionId, bool isAIGame)
    {
        Board = board;
        Player1 = player1;
        Player2 = player2;
        CurrentPlayer = player1;
        Player1ConnectionId = player1ConnectionId;
        Player2ConnectionId = player2ConnectionId;
        IsAIGame = isAIGame;
    }

    /// <summary>
    /// Switches to the other player.
    /// </summary>
    public void SwitchPlayer()
    {
        CurrentPlayer = CurrentPlayer == Player1 ? Player2 : Player1;
    }

    /// <summary>
    /// Converts the game session to a GameState for serialization.
    /// </summary>
    public GameState ToGameState()
    {
        var layers = Board.Is3D ? Board.Layers : 1;
        var boardData = new char[layers * Board.Size * Board.Size];
        
        for (int layer = 0; layer < layers; layer++)
        {
            for (int row = 0; row < Board.Size; row++)
            {
                for (int col = 0; col < Board.Size; col++)
                {
                    var index = layer * Board.Size * Board.Size + row * Board.Size + col;
                    boardData[index] = Board.GetMark(row, col, layer);
                }
            }
        }

        return new GameState
        {
            GameId = GameId,
            BoardSize = Board.Size,
            WinCondition = Board.WinCondition,
            Is3D = Board.Is3D,
            Layers = layers,
            BoardData = boardData,
            Player1ConnectionId = Player1ConnectionId,
            Player1Username = Player1.Name,
            Player1Mark = Player1.Mark,
            Player2ConnectionId = Player2ConnectionId,
            Player2Username = Player2.Name,
            Player2Mark = Player2.Mark,
            CurrentPlayerConnectionId = CurrentPlayer == Player1 ? Player1ConnectionId : Player2ConnectionId,
            IsAIGame = IsAIGame,
            IsGameOver = IsGameOver,
            WinnerConnectionId = Winner == Player1 ? Player1ConnectionId : (Winner == Player2 ? Player2ConnectionId : null),
            ResultMessage = IsGameOver ? (Winner != null ? $"{Winner.Name} wins!" : "It's a draw!") : null
        };
    }
}
