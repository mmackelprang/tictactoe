namespace TicTacToe;

/// <summary>
/// Abstract base class for all player types.
/// </summary>
public abstract class Player
{
    /// <summary>
    /// Gets the player's mark (typically 'X' or 'O').
    /// </summary>
    public char Mark { get; }

    /// <summary>
    /// Gets the player's name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the Player class.
    /// </summary>
    /// <param name="mark">The mark this player uses.</param>
    /// <param name="name">The name of the player.</param>
    protected Player(char mark, string name)
    {
        Mark = mark;
        Name = name;
    }

    /// <summary>
    /// Gets the player's next move.
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <returns>A tuple containing the row and column of the move.</returns>
    public abstract (int row, int col) GetMove(Board board);
}
