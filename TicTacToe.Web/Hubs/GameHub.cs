using Microsoft.AspNetCore.SignalR;
using TicTacToe.Web.Models;
using TicTacToe.Web.Services;

namespace TicTacToe.Web.Hubs;

/// <summary>
/// SignalR hub for real-time game communication.
/// </summary>
public class GameHub : Hub
{
    private readonly LobbyService _lobbyService;
    private readonly GameService _gameService;

    public GameHub(LobbyService lobbyService, GameService gameService)
    {
        _lobbyService = lobbyService;
        _gameService = gameService;
    }

    /// <summary>
    /// Called when a client joins the lobby.
    /// </summary>
    /// <param name="username">The user's username.</param>
    public async Task JoinLobby(string username)
    {
        // Check if username is already taken
        if (_lobbyService.IsUsernameTaken(username))
        {
            await Clients.Caller.SendAsync("Error", "Username is already taken");
            return;
        }

        // Add user to lobby
        if (_lobbyService.AddUser(Context.ConnectionId, username))
        {
            // Notify the user they joined successfully
            await Clients.Caller.SendAsync("JoinedLobby", username);

            // Notify all clients about the new user
            await Clients.All.SendAsync("OnUserConnected", Context.ConnectionId, username);

            // Send the updated user list to all clients
            var users = _lobbyService.GetAllUsers();
            await Clients.All.SendAsync("OnLobbyUpdated", users);
        }
    }

    /// <summary>
    /// Called when a client leaves the lobby.
    /// </summary>
    public async Task LeaveLobby()
    {
        var user = _lobbyService.GetUser(Context.ConnectionId);
        if (user != null)
        {
            _lobbyService.RemoveUser(Context.ConnectionId);
            
            // Notify all clients about the disconnection
            await Clients.Others.SendAsync("OnUserDisconnected", Context.ConnectionId);

            // Send the updated user list to all clients
            var users = _lobbyService.GetAllUsers();
            await Clients.All.SendAsync("OnLobbyUpdated", users);
        }
    }

    /// <summary>
    /// Called when a player invites another player to a game.
    /// </summary>
    /// <param name="targetConnectionId">The target player's connection ID.</param>
    public async Task InvitePlayer(string targetConnectionId)
    {
        var inviter = _lobbyService.GetUser(Context.ConnectionId);
        var target = _lobbyService.GetUser(targetConnectionId);

        if (inviter == null || target == null)
        {
            await Clients.Caller.SendAsync("Error", "User not found");
            return;
        }

        if (target.Status != UserStatus.Available)
        {
            await Clients.Caller.SendAsync("Error", "User is not available");
            return;
        }

        // Update statuses
        _lobbyService.UpdateUserStatus(Context.ConnectionId, UserStatus.PendingInvitation);
        _lobbyService.UpdateUserStatus(targetConnectionId, UserStatus.PendingInvitation);

        // Send invitation to target
        await Clients.Client(targetConnectionId).SendAsync("OnGameInvitation", Context.ConnectionId, inviter.Username);

        // Notify lobby of status changes
        var users = _lobbyService.GetAllUsers();
        await Clients.All.SendAsync("OnLobbyUpdated", users);
    }

    /// <summary>
    /// Called when a player accepts a game invitation.
    /// </summary>
    /// <param name="inviterConnectionId">The inviter's connection ID.</param>
    public async Task AcceptInvitation(string inviterConnectionId)
    {
        var inviter = _lobbyService.GetUser(inviterConnectionId);
        var accepter = _lobbyService.GetUser(Context.ConnectionId);

        if (inviter == null || accepter == null)
        {
            await Clients.Caller.SendAsync("Error", "User not found");
            return;
        }

        // Create the game
        var game = _gameService.CreatePlayerVsPlayerGame(
            inviterConnectionId,
            inviter.Username,
            Context.ConnectionId,
            accepter.Username
        );

        // Update user statuses
        _lobbyService.UpdateUserStatus(inviterConnectionId, UserStatus.InGame, game.GameId);
        _lobbyService.UpdateUserStatus(Context.ConnectionId, UserStatus.InGame, game.GameId);

        // Send game started notification to both players
        var gameState = game.ToGameState();
        await Clients.Clients(inviterConnectionId, Context.ConnectionId).SendAsync("OnGameStarted", gameState);

        // Notify lobby of status changes
        var users = _lobbyService.GetAllUsers();
        await Clients.All.SendAsync("OnLobbyUpdated", users);
    }

    /// <summary>
    /// Called when a player declines a game invitation.
    /// </summary>
    /// <param name="inviterConnectionId">The inviter's connection ID.</param>
    public async Task DeclineInvitation(string inviterConnectionId)
    {
        // Reset statuses back to available
        _lobbyService.UpdateUserStatus(inviterConnectionId, UserStatus.Available);
        _lobbyService.UpdateUserStatus(Context.ConnectionId, UserStatus.Available);

        // Notify the inviter
        await Clients.Client(inviterConnectionId).SendAsync("OnInvitationDeclined", Context.ConnectionId);

        // Notify lobby of status changes
        var users = _lobbyService.GetAllUsers();
        await Clients.All.SendAsync("OnLobbyUpdated", users);
    }

    /// <summary>
    /// Called when a player starts a game against AI.
    /// </summary>
    public async Task StartGameWithAI()
    {
        var user = _lobbyService.GetUser(Context.ConnectionId);
        if (user == null)
        {
            await Clients.Caller.SendAsync("Error", "User not found");
            return;
        }

        // Create the game
        var game = _gameService.CreatePlayerVsAIGame(Context.ConnectionId, user.Username);

        // Update user status
        _lobbyService.UpdateUserStatus(Context.ConnectionId, UserStatus.InGame, game.GameId);

        // Send game started notification
        var gameState = game.ToGameState();
        await Clients.Caller.SendAsync("OnGameStarted", gameState);

        // Notify lobby of status change
        var users = _lobbyService.GetAllUsers();
        await Clients.All.SendAsync("OnLobbyUpdated", users);
    }

    /// <summary>
    /// Called when a player makes a move.
    /// </summary>
    /// <param name="gameId">The game ID.</param>
    /// <param name="row">The row position.</param>
    /// <param name="col">The column position.</param>
    public async Task MakeMove(string gameId, int row, int col)
    {
        var game = _gameService.GetGame(gameId);
        if (game == null)
        {
            await Clients.Caller.SendAsync("Error", "Game not found");
            return;
        }

        // Make the move
        if (!_gameService.MakeMove(gameId, row, col, Context.ConnectionId))
        {
            await Clients.Caller.SendAsync("Error", "Invalid move");
            return;
        }

        // Get current player's mark (the one who just moved)
        var mark = game.CurrentPlayer == game.Player1 ? game.Player2.Mark : game.Player1.Mark;

        // Broadcast the move to both players
        if (game.IsAIGame)
        {
            await Clients.Caller.SendAsync("OnMoveMade", row, col, mark);
        }
        else
        {
            await Clients.Clients(game.Player1ConnectionId, game.Player2ConnectionId)
                .SendAsync("OnMoveMade", row, col, mark);
        }

        // Check if game is over
        if (game.IsGameOver)
        {
            var winnerConnectionId = game.Winner == game.Player1 
                ? game.Player1ConnectionId 
                : (game.Winner == game.Player2 ? game.Player2ConnectionId : null);

            var resultMessage = game.Winner != null ? $"{game.Winner.Name} wins!" : "It's a draw!";

            if (game.IsAIGame)
            {
                await Clients.Caller.SendAsync("OnGameEnded", winnerConnectionId, resultMessage);
            }
            else
            {
                await Clients.Clients(game.Player1ConnectionId, game.Player2ConnectionId)
                    .SendAsync("OnGameEnded", winnerConnectionId, resultMessage);
            }

            // Update user statuses back to available
            _lobbyService.UpdateUserStatus(game.Player1ConnectionId, UserStatus.Available);
            if (!game.IsAIGame)
            {
                _lobbyService.UpdateUserStatus(game.Player2ConnectionId, UserStatus.Available);
            }

            // Remove the game
            _gameService.RemoveGame(gameId);

            // Notify lobby
            var users = _lobbyService.GetAllUsers();
            await Clients.All.SendAsync("OnLobbyUpdated", users);
        }
        else if (game.IsAIGame && game.CurrentPlayer == game.Player2)
        {
            // It's AI's turn - make AI move
            var aiMove = _gameService.GetAIMove(gameId);
            if (aiMove.HasValue)
            {
                // Small delay for better UX
                await Task.Delay(500);

                // Make AI move
                if (_gameService.MakeMove(gameId, aiMove.Value.row, aiMove.Value.col, string.Empty))
                {
                    // Broadcast AI's move
                    await Clients.Caller.SendAsync("OnMoveMade", aiMove.Value.row, aiMove.Value.col, game.Player2.Mark);

                    // Check if game is over after AI move
                    if (game.IsGameOver)
                    {
                        var winnerConnectionId = game.Winner == game.Player1 
                            ? game.Player1ConnectionId 
                            : (game.Winner == game.Player2 ? string.Empty : null);

                        var resultMessage = game.Winner != null ? $"{game.Winner.Name} wins!" : "It's a draw!";
                        await Clients.Caller.SendAsync("OnGameEnded", winnerConnectionId, resultMessage);

                        // Update user status
                        _lobbyService.UpdateUserStatus(game.Player1ConnectionId, UserStatus.Available);

                        // Remove the game
                        _gameService.RemoveGame(gameId);

                        // Notify lobby
                        var users = _lobbyService.GetAllUsers();
                        await Clients.All.SendAsync("OnLobbyUpdated", users);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Called when a player leaves a game.
    /// </summary>
    /// <param name="gameId">The game ID.</param>
    public async Task LeaveGame(string gameId)
    {
        var game = _gameService.GetGame(gameId);
        if (game == null)
            return;

        // Determine who left and who remains
        var leaverConnectionId = Context.ConnectionId;
        var remainingConnectionId = game.Player1ConnectionId == leaverConnectionId 
            ? game.Player2ConnectionId 
            : game.Player1ConnectionId;

        // Notify the other player if not AI game
        if (!game.IsAIGame && !string.IsNullOrEmpty(remainingConnectionId))
        {
            await Clients.Client(remainingConnectionId).SendAsync("OnGameEnded", null, "Opponent left the game");
            _lobbyService.UpdateUserStatus(remainingConnectionId, UserStatus.Available);
        }

        // Update leaver status
        _lobbyService.UpdateUserStatus(leaverConnectionId, UserStatus.Available);

        // Remove the game
        _gameService.RemoveGame(gameId);

        // Notify lobby
        var users = _lobbyService.GetAllUsers();
        await Clients.All.SendAsync("OnLobbyUpdated", users);
    }

    /// <summary>
    /// Called when a client disconnects.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = _lobbyService.GetUser(Context.ConnectionId);
        if (user != null)
        {
            // If user was in a game, end it
            if (!string.IsNullOrEmpty(user.CurrentGameId))
            {
                await LeaveGame(user.CurrentGameId);
            }

            // Remove from lobby
            _lobbyService.RemoveUser(Context.ConnectionId);

            // Notify all clients
            await Clients.Others.SendAsync("OnUserDisconnected", Context.ConnectionId);

            var users = _lobbyService.GetAllUsers();
            await Clients.All.SendAsync("OnLobbyUpdated", users);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
