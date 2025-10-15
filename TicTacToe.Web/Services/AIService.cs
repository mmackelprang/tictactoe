using TicTacToe;

namespace TicTacToe.Web.Services;

/// <summary>
/// Represents the AI difficulty level.
/// </summary>
public enum AIDifficulty
{
    /// <summary>
    /// Easy difficulty - uses current ComputerPlayer logic (win/block/random).
    /// </summary>
    Easy,
    
    /// <summary>
    /// Medium difficulty - minimax with depth limit.
    /// </summary>
    Medium,
    
    /// <summary>
    /// Hard difficulty - full minimax with alpha-beta pruning (optimal play).
    /// </summary>
    Hard
}

/// <summary>
/// Provides AI move calculation service with multiple difficulty levels.
/// </summary>
public class AIService
{
    /// <summary>
    /// Gets the AI's next move using the specified difficulty level.
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <param name="mark">The AI's mark.</param>
    /// <param name="difficulty">The difficulty level (defaults to Medium).</param>
    /// <returns>A tuple containing the row and column of the move.</returns>
    public (int row, int col) GetAIMove(Board board, char mark, AIDifficulty difficulty = AIDifficulty.Medium)
    {
        Player aiPlayer;
        
        if (difficulty == AIDifficulty.Easy)
        {
            // Use the basic ComputerPlayer for easy difficulty
            aiPlayer = new ComputerPlayer(mark, "AI");
        }
        else
        {
            // Use MinimaxAIPlayer for medium and hard difficulty
            var minimaxDifficulty = difficulty == AIDifficulty.Medium 
                ? MinimaxAIPlayer.Difficulty.Medium 
                : MinimaxAIPlayer.Difficulty.Hard;
            aiPlayer = new MinimaxAIPlayer(mark, "AI", minimaxDifficulty);
        }
        
        return aiPlayer.GetMove(board);
    }
    
    /// <summary>
    /// Gets the AI's next move using the ComputerPlayer logic (backward compatibility).
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <param name="mark">The AI's mark.</param>
    /// <returns>A tuple containing the row and column of the move.</returns>
    public (int row, int col) GetAIMove(Board board, char mark)
    {
        return GetAIMove(board, mark, AIDifficulty.Medium);
    }
}
