using TicTacToe;

namespace TicTacToe.Web.Services;

/// <summary>
/// Provides AI move calculation service.
/// </summary>
public class AIService
{
    /// <summary>
    /// Gets the AI's next move using the ComputerPlayer logic.
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <param name="mark">The AI's mark.</param>
    /// <returns>A tuple containing the row and column of the move.</returns>
    public (int row, int col) GetAIMove(Board board, char mark)
    {
        // Create a temporary computer player to get the move
        var aiPlayer = new ComputerPlayer(mark, "AI");
        return aiPlayer.GetMove(board);
    }
}
