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
    /// Gets the computer's next move using a simple random strategy.
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <returns>A tuple containing the row and column of the move.</returns>
    public override (int row, int col) GetMove(Board board)
    {
        Console.WriteLine($"\n{Name} is thinking...");
        Thread.Sleep(500); // Small delay for better UX

        // Collect all empty positions
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

        // Select a random empty position
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
}
