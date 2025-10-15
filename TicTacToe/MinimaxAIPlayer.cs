namespace TicTacToe;

/// <summary>
/// Represents an AI player that uses the Minimax algorithm with optional alpha-beta pruning.
/// </summary>
public class MinimaxAIPlayer : Player
{
    private readonly int _maxDepth;
    private readonly bool _useAlphaBeta;
    private char _opponentMark;

    /// <summary>
    /// Represents the AI difficulty level.
    /// </summary>
    public enum Difficulty
    {
        /// <summary>
        /// Easy difficulty - uses current ComputerPlayer logic (win/block/random).
        /// </summary>
        Easy,
        
        /// <summary>
        /// Medium difficulty - minimax with depth limit of 4.
        /// </summary>
        Medium,
        
        /// <summary>
        /// Hard difficulty - full minimax with alpha-beta pruning (optimal play).
        /// </summary>
        Hard
    }

    /// <summary>
    /// Initializes a new instance of the MinimaxAIPlayer class.
    /// </summary>
    /// <param name="mark">The mark this player uses.</param>
    /// <param name="name">The name of the player.</param>
    /// <param name="difficulty">The difficulty level.</param>
    public MinimaxAIPlayer(char mark, string name, Difficulty difficulty = Difficulty.Hard) 
        : base(mark, name)
    {
        _opponentMark = mark == 'X' ? 'O' : 'X';
        
        // Configure based on difficulty
        switch (difficulty)
        {
            case Difficulty.Easy:
                _maxDepth = 1;
                _useAlphaBeta = false;
                break;
            case Difficulty.Medium:
                _maxDepth = 4;
                _useAlphaBeta = false;
                break;
            case Difficulty.Hard:
                _maxDepth = int.MaxValue; // No depth limit for hard
                _useAlphaBeta = true;
                break;
            default:
                _maxDepth = int.MaxValue;
                _useAlphaBeta = true;
                break;
        }
    }

    /// <summary>
    /// Initializes a new instance of the MinimaxAIPlayer class with custom parameters.
    /// </summary>
    /// <param name="mark">The mark this player uses.</param>
    /// <param name="name">The name of the player.</param>
    /// <param name="maxDepth">Maximum search depth.</param>
    /// <param name="useAlphaBeta">Whether to use alpha-beta pruning.</param>
    public MinimaxAIPlayer(char mark, string name, int maxDepth, bool useAlphaBeta) 
        : base(mark, name)
    {
        _maxDepth = maxDepth;
        _useAlphaBeta = useAlphaBeta;
        _opponentMark = mark == 'X' ? 'O' : 'X';
    }

    /// <summary>
    /// Gets the AI's next move using minimax algorithm.
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <returns>A tuple containing the row and column of the move.</returns>
    public override (int row, int col) GetMove(Board board)
    {
        if (_useAlphaBeta)
            return GetBestMoveWithAlphaBeta(board);
        else
            return GetBestMoveWithMinimax(board);
    }

    /// <summary>
    /// Finds the best move using minimax algorithm.
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <returns>The best move as (row, col).</returns>
    private (int row, int col) GetBestMoveWithMinimax(Board board)
    {
        int bestScore = int.MinValue;
        (int row, int col) bestMove = (-1, -1);

        // Try all possible moves
        for (int row = 0; row < board.Size; row++)
        {
            for (int col = 0; col < board.Size; col++)
            {
                if (board.IsEmpty(row, col))
                {
                    // Make the move
                    board.PlaceMark(row, col, Mark);
                    
                    // Evaluate this move
                    int score = Minimax(board, 0, false);
                    
                    // Undo the move
                    board.ClearPosition(row, col);
                    
                    // Update best move if this is better
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = (row, col);
                    }
                }
            }
        }

        return bestMove;
    }

    /// <summary>
    /// Finds the best move using minimax with alpha-beta pruning.
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <returns>The best move as (row, col).</returns>
    private (int row, int col) GetBestMoveWithAlphaBeta(Board board)
    {
        int bestScore = int.MinValue;
        (int row, int col) bestMove = (-1, -1);
        int alpha = int.MinValue;
        int beta = int.MaxValue;

        // Try all possible moves
        for (int row = 0; row < board.Size; row++)
        {
            for (int col = 0; col < board.Size; col++)
            {
                if (board.IsEmpty(row, col))
                {
                    // Make the move
                    board.PlaceMark(row, col, Mark);
                    
                    // Evaluate this move
                    int score = MinimaxWithAlphaBeta(board, 0, alpha, beta, false);
                    
                    // Undo the move
                    board.ClearPosition(row, col);
                    
                    // Update best move if this is better
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = (row, col);
                    }
                    
                    // Update alpha
                    alpha = Math.Max(alpha, score);
                }
            }
        }

        return bestMove;
    }

    /// <summary>
    /// Minimax algorithm for finding the optimal move.
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <param name="depth">Current search depth.</param>
    /// <param name="isMaximizing">True if maximizing player's turn.</param>
    /// <returns>The score of the position.</returns>
    private int Minimax(Board board, int depth, bool isMaximizing)
    {
        // Check terminal states
        if (board.CheckWin(Mark))
            return 10 - depth; // Prefer faster wins
        if (board.CheckWin(_opponentMark))
            return depth - 10; // Prefer slower losses
        if (board.IsFull())
            return 0; // Draw
        if (depth >= _maxDepth)
            return EvaluatePosition(board); // Depth limit reached

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            
            // Try all possible moves
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.IsEmpty(row, col))
                    {
                        board.PlaceMark(row, col, Mark);
                        int score = Minimax(board, depth + 1, false);
                        board.ClearPosition(row, col);
                        bestScore = Math.Max(score, bestScore);
                    }
                }
            }
            
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            
            // Try all possible moves for opponent
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.IsEmpty(row, col))
                    {
                        board.PlaceMark(row, col, _opponentMark);
                        int score = Minimax(board, depth + 1, true);
                        board.ClearPosition(row, col);
                        bestScore = Math.Min(score, bestScore);
                    }
                }
            }
            
            return bestScore;
        }
    }

    /// <summary>
    /// Minimax algorithm with alpha-beta pruning.
    /// </summary>
    /// <param name="board">The current game board.</param>
    /// <param name="depth">Current search depth.</param>
    /// <param name="alpha">Alpha value for pruning.</param>
    /// <param name="beta">Beta value for pruning.</param>
    /// <param name="isMaximizing">True if maximizing player's turn.</param>
    /// <returns>The score of the position.</returns>
    private int MinimaxWithAlphaBeta(Board board, int depth, int alpha, int beta, bool isMaximizing)
    {
        // Check terminal states
        if (board.CheckWin(Mark))
            return 10 - depth; // Prefer faster wins
        if (board.CheckWin(_opponentMark))
            return depth - 10; // Prefer slower losses
        if (board.IsFull())
            return 0; // Draw
        if (depth >= _maxDepth)
            return EvaluatePosition(board); // Depth limit reached

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            
            // Try all possible moves
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.IsEmpty(row, col))
                    {
                        board.PlaceMark(row, col, Mark);
                        int score = MinimaxWithAlphaBeta(board, depth + 1, alpha, beta, false);
                        board.ClearPosition(row, col);
                        bestScore = Math.Max(score, bestScore);
                        alpha = Math.Max(alpha, score);
                        
                        // Beta cutoff
                        if (beta <= alpha)
                            break;
                    }
                }
                
                // Beta cutoff (outer loop)
                if (beta <= alpha)
                    break;
            }
            
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            
            // Try all possible moves for opponent
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.IsEmpty(row, col))
                    {
                        board.PlaceMark(row, col, _opponentMark);
                        int score = MinimaxWithAlphaBeta(board, depth + 1, alpha, beta, true);
                        board.ClearPosition(row, col);
                        bestScore = Math.Min(score, bestScore);
                        beta = Math.Min(beta, score);
                        
                        // Alpha cutoff
                        if (beta <= alpha)
                            break;
                    }
                }
                
                // Alpha cutoff (outer loop)
                if (beta <= alpha)
                    break;
            }
            
            return bestScore;
        }
    }

    /// <summary>
    /// Evaluates a non-terminal position using heuristics.
    /// Used when depth limit is reached.
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <returns>Heuristic score for the position.</returns>
    private int EvaluatePosition(Board board)
    {
        int score = 0;
        
        // Evaluate center control (center positions are more valuable)
        int center = board.Size / 2;
        if (!board.IsEmpty(center, center))
        {
            if (board.GetMark(center, center) == Mark)
                score += 3;
            else
                score -= 3;
        }
        
        // Evaluate corners (corners are valuable)
        int[][] corners = new int[][] {
            new int[] { 0, 0 },
            new int[] { 0, board.Size - 1 },
            new int[] { board.Size - 1, 0 },
            new int[] { board.Size - 1, board.Size - 1 }
        };
        
        foreach (var corner in corners)
        {
            if (!board.IsEmpty(corner[0], corner[1]))
            {
                if (board.GetMark(corner[0], corner[1]) == Mark)
                    score += 2;
                else
                    score -= 2;
            }
        }
        
        // Count potential winning lines
        score += CountPotentialLines(board, Mark) * 1;
        score -= CountPotentialLines(board, _opponentMark) * 1;
        
        return score;
    }

    /// <summary>
    /// Counts the number of potential winning lines for a player.
    /// A potential line is one that isn't blocked by the opponent.
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <param name="mark">The mark to evaluate.</param>
    /// <returns>Number of potential winning lines.</returns>
    private int CountPotentialLines(Board board, char mark)
    {
        int count = 0;
        char opponent = mark == 'X' ? 'O' : 'X';
        
        // Check rows
        for (int row = 0; row < board.Size; row++)
        {
            for (int col = 0; col <= board.Size - board.WinCondition; col++)
            {
                if (IsLinePotential(board, row, col, 0, 1, mark, opponent))
                    count++;
            }
        }
        
        // Check columns
        for (int col = 0; col < board.Size; col++)
        {
            for (int row = 0; row <= board.Size - board.WinCondition; row++)
            {
                if (IsLinePotential(board, row, col, 1, 0, mark, opponent))
                    count++;
            }
        }
        
        // Check diagonals
        for (int row = 0; row <= board.Size - board.WinCondition; row++)
        {
            for (int col = 0; col <= board.Size - board.WinCondition; col++)
            {
                if (IsLinePotential(board, row, col, 1, 1, mark, opponent))
                    count++;
            }
        }
        
        // Check anti-diagonals
        for (int row = 0; row <= board.Size - board.WinCondition; row++)
        {
            for (int col = board.WinCondition - 1; col < board.Size; col++)
            {
                if (IsLinePotential(board, row, col, 1, -1, mark, opponent))
                    count++;
            }
        }
        
        return count;
    }

    /// <summary>
    /// Checks if a line is potentially winnable (not blocked by opponent).
    /// </summary>
    /// <param name="board">The game board.</param>
    /// <param name="startRow">Starting row.</param>
    /// <param name="startCol">Starting column.</param>
    /// <param name="deltaRow">Row direction.</param>
    /// <param name="deltaCol">Column direction.</param>
    /// <param name="mark">The mark to check.</param>
    /// <param name="opponent">The opponent's mark.</param>
    /// <returns>True if the line is potential, false otherwise.</returns>
    private bool IsLinePotential(Board board, int startRow, int startCol, int deltaRow, int deltaCol, char mark, char opponent)
    {
        bool hasOurMark = false;
        
        for (int i = 0; i < board.WinCondition; i++)
        {
            int row = startRow + i * deltaRow;
            int col = startCol + i * deltaCol;
            char cell = board.GetMark(row, col);
            
            // If opponent has a mark in this line, it's blocked
            if (cell == opponent)
                return false;
            
            // Track if we have at least one mark
            if (cell == mark)
                hasOurMark = true;
        }
        
        return hasOurMark;
    }
}
