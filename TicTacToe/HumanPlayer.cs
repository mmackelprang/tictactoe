namespace TicTacToe;

/// <summary>
/// Represents a human player who inputs moves via the console.
/// </summary>
public class HumanPlayer : Player
{
    /// <summary>
    /// Initializes a new instance of the HumanPlayer class.
    /// </summary>
    /// <param name="mark">The mark this player uses.</param>
    /// <param name="name">The name of the player.</param>
    public HumanPlayer(char mark, string name) : base(mark, name)
    {
    }

    /// <summary>
    /// Gets the player's next move by prompting for console input.
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <returns>A tuple containing the row and column of the move.</returns>
    public override (int row, int col) GetMove(Board board)
    {
        while (true)
        {
            Console.WriteLine($"\n{Name}'s turn ({Mark})");
            Console.Write("Enter row: ");
            string? rowInput = Console.ReadLine();
            
            Console.Write("Enter column: ");
            string? colInput = Console.ReadLine();

            if (int.TryParse(rowInput, out int row) && 
                int.TryParse(colInput, out int col))
            {
                if (board.IsValidPosition(row, col))
                {
                    if (board.IsEmpty(row, col))
                    {
                        return (row, col);
                    }
                    else
                    {
                        Console.WriteLine("That position is already occupied. Try again.");
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid position. Please enter values between 0 and {board.Size - 1}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter numeric values.");
            }
        }
    }
}
