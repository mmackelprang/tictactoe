namespace TicTacToe;

/// <summary>
/// Controls the game flow and manages the game state.
/// </summary>
public class GameController
{
    private readonly Board _board;
    private readonly Player _player1;
    private readonly Player _player2;
    private Player _currentPlayer;

    /// <summary>
    /// Initializes a new instance of the GameController class.
    /// </summary>
    /// <param name="player1">The first player.</param>
    /// <param name="player2">The second player.</param>
    /// <param name="boardSize">The size of the board (default is 3).</param>
    /// <param name="winCondition">The number of consecutive marks needed to win (default is 3).</param>
    public GameController(Player player1, Player player2, int boardSize = 3, int winCondition = 3)
    {
        _board = new Board(boardSize, winCondition);
        _player1 = player1;
        _player2 = player2;
        _currentPlayer = player1;
    }

    /// <summary>
    /// Starts and runs the main game loop.
    /// </summary>
    public void PlayGame()
    {
        Console.Clear();
        Console.WriteLine("=== TIC-TAC-TOE ===");
        Console.WriteLine($"Board Size: {_board.Size}x{_board.Size}");
        Console.WriteLine($"Win Condition: {_board.WinCondition} in a row");
        Console.WriteLine($"{_player1.Name} ({_player1.Mark}) vs {_player2.Name} ({_player2.Mark})");
        Console.WriteLine("\nPress any key to start...");
        Console.ReadKey();

        while (true)
        {
            // Display the board
            Console.Clear();
            Console.WriteLine(_board.ToString());

            // Get and execute the current player's move
            var (row, col) = _currentPlayer.GetMove(_board);
            
            if (!_board.PlaceMark(row, col, _currentPlayer.Mark))
            {
                Console.WriteLine("Failed to place mark. This shouldn't happen!");
                continue;
            }

            // Check for win
            if (_board.CheckWin(_currentPlayer.Mark))
            {
                Console.Clear();
                Console.WriteLine(_board.ToString());
                Console.WriteLine($"\n🎉 {_currentPlayer.Name} ({_currentPlayer.Mark}) wins!");
                break;
            }

            // Check for draw
            if (_board.IsFull())
            {
                Console.Clear();
                Console.WriteLine(_board.ToString());
                Console.WriteLine("\n🤝 It's a draw!");
                break;
            }

            // Switch to the other player
            SwitchPlayer();
        }

        Console.WriteLine("\nGame over! Press any key to exit...");
        Console.ReadKey();
    }

    /// <summary>
    /// Switches the current player to the other player.
    /// </summary>
    private void SwitchPlayer()
    {
        _currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;
    }
}
