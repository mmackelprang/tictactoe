namespace TicTacToe;

/// <summary>
/// Represents a computer player that makes moves automatically.
/// </summary>
public class ComputerPlayer : Player
{
    private readonly Random _random;

    /// <summary>
    /// Initializes a new instance of the ComputerPlayer class.
    /// </summary>
    /// <param name="mark">The mark this player uses.</param>
    /// <param name="name">The name of the player.</param>
    public ComputerPlayer(char mark, string name) : base(mark, name)
    {
        _random = new Random();
    }

    /// <summary>
    /// Gets the computer's next move using a smart strategy.
    /// Priority: 1) Win if possible, 2) Block opponent's win, 3) Random move
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <returns>A tuple containing the row and column of the move.</returns>
    public override (int row, int col) GetMove(Board board)
    {
        Console.WriteLine($"\n{Name} is thinking...");
        Thread.Sleep(500); // Small delay for better UX

        // Step 1: Check if we can win in the next move
        var winningMove = FindWinningMove(board, Mark);
        if (winningMove.HasValue)
        {
            Console.WriteLine($"{Name} chooses position ({winningMove.Value.row}, {winningMove.Value.col}) - Winning move!");
            return winningMove.Value;
        }

        // Step 2: Check if opponent can win and block them
        char opponentMark = Mark == 'X' ? 'O' : 'X';
        var blockingMove = FindWinningMove(board, opponentMark);
        if (blockingMove.HasValue)
        {
            Console.WriteLine($"{Name} chooses position ({blockingMove.Value.row}, {blockingMove.Value.col}) - Blocking opponent!");
            return blockingMove.Value;
        }

        // Step 3: Make a random move
        var emptyPositions = new List<(int row, int col)>();
        for (int row = 0; row < board.Size; row++)
        {
            for (int col = 0; col < board.Size; col++)
            {
                if (board.IsEmpty(row, col))
                {
                    emptyPositions.Add((row, col));
                }
            }
        }

        if (emptyPositions.Count > 0)
        {
            int index = _random.Next(emptyPositions.Count);
            var move = emptyPositions[index];
            Console.WriteLine($"{Name} chooses position ({move.row}, {move.col})");
            return move;
        }

        // This should never happen in a valid game
        throw new InvalidOperationException("No valid moves available.");
    }

    /// <summary>
    /// Finds a move that would result in a win for the specified player.
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <param name="mark">The mark to check for winning moves.</param>
    /// <returns>A winning move if found, null otherwise.</returns>
    private (int row, int col)? FindWinningMove(Board board, char mark)
    {
        // Try each empty position to see if it results in a win
        for (int row = 0; row < board.Size; row++)
        {
            for (int col = 0; col < board.Size; col++)
            {
                if (board.IsEmpty(row, col))
                {
                    // Temporarily place the mark
                    board.PlaceMark(row, col, mark);
                    
                    // Check if this results in a win
                    bool isWin = board.CheckWin(mark);
                    
                    // Remove the mark (undo the move)
                    board.ClearPosition(row, col);
                    
                    if (isWin)
                    {
                        return (row, col);
                    }
                }
            }
        }
        
        return null;
    }
}
