using System.Collections.Concurrent;
using TicTacToe;
using TicTacToe.Web.Models;

namespace TicTacToe.Web.Services;

/// <summary>
/// Manages multiple game instances and game logic.
/// </summary>
public class GameService
{
    private readonly ConcurrentDictionary<string, GameSession> _games = new();
    private readonly AIService _aiService;

    public GameService(AIService aiService)
    {
        _aiService = aiService;
    }

    /// <summary>
    /// Creates a new game between two players.
    /// </summary>
    /// <param name="player1ConnectionId">Player 1's connection ID.</param>
    /// <param name="player1Username">Player 1's username.</param>
    /// <param name="player2ConnectionId">Player 2's connection ID.</param>
    /// <param name="player2Username">Player 2's username.</param>
    /// <param name="boardSize">The board size.</param>
    /// <param name="winCondition">The win condition.</param>
    /// <returns>The created game session.</returns>
    public GameSession CreatePlayerVsPlayerGame(
        string player1ConnectionId, 
        string player1Username,
        string player2ConnectionId,
        string player2Username,
        int boardSize = 3,
        int winCondition = 3)
    {
        var board = new Board(boardSize, winCondition);
        var player1 = new NetworkPlayer('X', player1Username);
        var player2 = new NetworkPlayer('O', player2Username);

        var gameSession = new GameSession(
            board,
            player1,
            player2,
            player1ConnectionId,
            player2ConnectionId,
            false
        );

        _games.TryAdd(gameSession.GameId, gameSession);
        return gameSession;
    }

    /// <summary>
    /// Creates a new game between a player and AI.
    /// </summary>
    /// <param name="playerConnectionId">Player's connection ID.</param>
    /// <param name="playerUsername">Player's username.</param>
    /// <param name="boardSize">The board size.</param>
    /// <param name="winCondition">The win condition.</param>
    /// <returns>The created game session.</returns>
    public GameSession CreatePlayerVsAIGame(
        string playerConnectionId,
        string playerUsername,
        int boardSize = 3,
        int winCondition = 3)
    {
        var board = new Board(boardSize, winCondition);
        var player = new NetworkPlayer('X', playerUsername);
        var aiPlayer = new ComputerPlayer('O', "AI");

        var gameSession = new GameSession(
            board,
            player,
            aiPlayer,
            playerConnectionId,
            string.Empty, // AI has no connection ID
            true
        );

        _games.TryAdd(gameSession.GameId, gameSession);
        return gameSession;
    }

    /// <summary>
    /// Gets a game by ID.
    /// </summary>
    /// <param name="gameId">The game ID.</param>
    /// <returns>The game session or null if not found.</returns>
    public GameSession? GetGame(string gameId)
    {
        _games.TryGetValue(gameId, out var game);
        return game;
    }

    /// <summary>
    /// Makes a move in a game.
    /// </summary>
    /// <param name="gameId">The game ID.</param>
    /// <param name="row">The row position.</param>
    /// <param name="col">The column position.</param>
    /// <param name="connectionId">The connection ID of the player making the move.</param>
    /// <returns>True if the move was successful.</returns>
    public bool MakeMove(string gameId, int row, int col, string connectionId)
    {
        var game = GetGame(gameId);
        if (game == null || game.IsGameOver)
            return false;

        // Verify it's this player's turn
        var currentPlayerConnectionId = game.CurrentPlayer == game.Player1 
            ? game.Player1ConnectionId 
            : game.Player2ConnectionId;

        if (currentPlayerConnectionId != connectionId)
            return false;

        // Make the move
        if (!game.Board.PlaceMark(row, col, game.CurrentPlayer.Mark))
            return false;

        // Check for win
        if (game.Board.CheckWin(game.CurrentPlayer.Mark))
        {
            game.IsGameOver = true;
            game.Winner = game.CurrentPlayer;
            return true;
        }

        // Check for draw
        if (game.Board.IsFull())
        {
            game.IsGameOver = true;
            game.Winner = null;
            return true;
        }

        // Switch player
        game.SwitchPlayer();
        return true;
    }

    /// <summary>
    /// Gets the AI's move for a game.
    /// </summary>
    /// <param name="gameId">The game ID.</param>
    /// <returns>The AI's move (row, col) or null if invalid.</returns>
    public (int row, int col)? GetAIMove(string gameId)
    {
        var game = GetGame(gameId);
        if (game == null || !game.IsAIGame || game.IsGameOver)
            return null;

        // Verify it's the AI's turn
        if (game.CurrentPlayer != game.Player2)
            return null;

        return _aiService.GetAIMove(game.Board, game.CurrentPlayer.Mark);
    }

    /// <summary>
    /// Removes a game from the active games.
    /// </summary>
    /// <param name="gameId">The game ID.</param>
    /// <returns>True if removed successfully.</returns>
    public bool RemoveGame(string gameId)
    {
        return _games.TryRemove(gameId, out _);
    }

    /// <summary>
    /// Updates a player's connection ID in a game (for handling reconnections).
    /// </summary>
    /// <param name="gameId">The game ID.</param>
    /// <param name="oldConnectionId">The old connection ID to replace.</param>
    /// <param name="newConnectionId">The new connection ID.</param>
    /// <returns>True if updated successfully.</returns>
    public bool UpdateConnectionId(string gameId, string oldConnectionId, string newConnectionId)
    {
        var game = GetGame(gameId);
        if (game == null)
            return false;

        if (game.Player1ConnectionId == oldConnectionId)
        {
            game.Player1ConnectionId = newConnectionId;
            return true;
        }
        else if (game.Player2ConnectionId == oldConnectionId)
        {
            game.Player2ConnectionId = newConnectionId;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets all active games.
    /// </summary>
    /// <returns>A list of all game sessions.</returns>
    public List<GameSession> GetAllGames()
    {
        return _games.Values.ToList();
    }

    /// <summary>
    /// Gets games for a specific connection ID.
    /// </summary>
    /// <param name="connectionId">The connection ID.</param>
    /// <returns>A list of games the user is in.</returns>
    public List<GameSession> GetGamesForConnection(string connectionId)
    {
        return _games.Values
            .Where(g => g.Player1ConnectionId == connectionId || g.Player2ConnectionId == connectionId)
            .ToList();
    }
}

/// <summary>
/// Represents a network player (human player over network).
/// </summary>
public class NetworkPlayer : Player
{
    public NetworkPlayer(char mark, string name) : base(mark, name)
    {
    }

    /// <summary>
    /// Network players don't use GetMove - moves come from SignalR.
    /// </summary>
    public override (int row, int col) GetMove(Board board)
    {
        throw new NotImplementedException("Network players make moves through SignalR");
    }
}
