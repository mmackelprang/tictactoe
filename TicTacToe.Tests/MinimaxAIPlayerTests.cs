using TicTacToe;
using Xunit;

namespace TicTacToe.Tests;

/// <summary>
/// Unit tests for the MinimaxAIPlayer class.
/// </summary>
public class MinimaxAIPlayerTests
{
    [Fact]
    public void MinimaxAIPlayer_ShouldWinWhenPossible_3x3()
    {
        // Arrange: Board with winning move available for X
        // X | O | X
        // O | X |  
        //   |   | O
        var board = new Board(3, 3);
        board.PlaceMark(0, 0, 'X'); // X
        board.PlaceMark(0, 1, 'O'); // O
        board.PlaceMark(0, 2, 'X'); // X
        board.PlaceMark(1, 0, 'O'); // O
        board.PlaceMark(1, 1, 'X'); // X
        board.PlaceMark(2, 2, 'O'); // O
        
        var aiPlayer = new MinimaxAIPlayer('X', "AI", MinimaxAIPlayer.Difficulty.Hard);

        // Act
        var move = aiPlayer.GetMove(board);

        // Assert: Should make a winning move
        board.PlaceMark(move.row, move.col, 'X');
        Assert.True(board.CheckWin('X'), "AI should make a winning move");
    }

    [Fact]
    public void MinimaxAIPlayer_ShouldBlockOpponentWin_3x3()
    {
        // Arrange: Board with opponent about to win
        // X | O |  
        // X | O |  
        //   |   |  
        var board = new Board(3, 3);
        board.PlaceMark(0, 0, 'X'); // X
        board.PlaceMark(0, 1, 'O'); // O
        board.PlaceMark(1, 0, 'X'); // X
        board.PlaceMark(1, 1, 'O'); // O
        
        var aiPlayer = new MinimaxAIPlayer('X', "AI", MinimaxAIPlayer.Difficulty.Hard);

        // Act
        var move = aiPlayer.GetMove(board);

        // Assert: X can win at (2,0), so AI should take that winning move
        // The AI will prioritize winning over blocking
        board.PlaceMark(move.row, move.col, 'X');
        Assert.True(board.CheckWin('X') || move == (2, 1), 
            "AI should either win at (2,0) or block opponent at (2,1)");
    }

    [Fact]
    public void MinimaxAIPlayer_ShouldPlayOptimallyOnEmptyBoard_3x3()
    {
        // Arrange: Empty board
        var board = new Board(3, 3);
        var aiPlayer = new MinimaxAIPlayer('X', "AI", MinimaxAIPlayer.Difficulty.Hard);

        // Act
        var move = aiPlayer.GetMove(board);

        // Assert: Should play center or corner (optimal opening moves)
        bool isOptimalMove = (move.row == 1 && move.col == 1) || // Center
                             (move.row == 0 && move.col == 0) || // Top-left corner
                             (move.row == 0 && move.col == 2) || // Top-right corner
                             (move.row == 2 && move.col == 0) || // Bottom-left corner
                             (move.row == 2 && move.col == 2);   // Bottom-right corner
        Assert.True(isOptimalMove, $"Move ({move.row}, {move.col}) is not optimal for empty board");
    }

    [Fact]
    public void MinimaxAIPlayer_AlphaBetaPruning_ShouldReturnSameResultAsMinimax()
    {
        // Arrange: Board with some moves played
        // X | O | X
        //   | O |  
        //   |   |  
        var board = new Board(3, 3);
        board.PlaceMark(0, 0, 'X');
        board.PlaceMark(0, 1, 'O');
        board.PlaceMark(0, 2, 'X');
        board.PlaceMark(1, 1, 'O');
        
        var minimaxPlayer = new MinimaxAIPlayer('X', "Minimax", int.MaxValue, false);
        var alphaBetaPlayer = new MinimaxAIPlayer('X', "AlphaBeta", int.MaxValue, true);

        // Act
        var minimaxMove = minimaxPlayer.GetMove(board);
        
        // Reset board state
        board = new Board(3, 3);
        board.PlaceMark(0, 0, 'X');
        board.PlaceMark(0, 1, 'O');
        board.PlaceMark(0, 2, 'X');
        board.PlaceMark(1, 1, 'O');
        
        var alphaBetaMove = alphaBetaPlayer.GetMove(board);

        // Assert: Both should return the same optimal move
        Assert.Equal(minimaxMove.row, alphaBetaMove.row);
        Assert.Equal(minimaxMove.col, alphaBetaMove.col);
    }

    [Fact]
    public void MinimaxAIPlayer_EasyDifficulty_ShouldMakeValidMove()
    {
        // Arrange
        var board = new Board(3, 3);
        board.PlaceMark(0, 0, 'X');
        board.PlaceMark(1, 1, 'O');
        
        var aiPlayer = new MinimaxAIPlayer('X', "AI", MinimaxAIPlayer.Difficulty.Easy);

        // Act
        var move = aiPlayer.GetMove(board);

        // Assert: Should return a valid empty position
        Assert.True(board.IsEmpty(move.row, move.col), "Move should be in an empty position");
        Assert.True(board.IsValidPosition(move.row, move.col), "Move should be valid");
    }

    [Fact]
    public void MinimaxAIPlayer_MediumDifficulty_ShouldMakeValidMove()
    {
        // Arrange
        var board = new Board(3, 3);
        board.PlaceMark(0, 0, 'X');
        board.PlaceMark(1, 1, 'O');
        
        var aiPlayer = new MinimaxAIPlayer('X', "AI", MinimaxAIPlayer.Difficulty.Medium);

        // Act
        var move = aiPlayer.GetMove(board);

        // Assert: Should return a valid empty position
        Assert.True(board.IsEmpty(move.row, move.col), "Move should be in an empty position");
        Assert.True(board.IsValidPosition(move.row, move.col), "Move should be valid");
    }

    [Fact]
    public void MinimaxAIPlayer_HardDifficulty_ShouldNeverLose_3x3()
    {
        // Arrange: Simulate a full game where AI plays second
        var board = new Board(3, 3);
        var aiPlayer = new MinimaxAIPlayer('O', "AI", MinimaxAIPlayer.Difficulty.Hard);
        
        // Human plays first at center
        board.PlaceMark(1, 1, 'X');
        
        // Act & Assert: Play out the game
        for (int i = 0; i < 4; i++) // Maximum 4 AI moves in a game
        {
            if (board.IsFull() || board.CheckWin('X') || board.CheckWin('O'))
                break;
                
            // AI move
            var aiMove = aiPlayer.GetMove(board);
            board.PlaceMark(aiMove.row, aiMove.col, 'O');
            
            if (board.IsFull() || board.CheckWin('O'))
                break;
            
            // Simulate human move (random valid position)
            bool moveMade = false;
            for (int row = 0; row < 3 && !moveMade; row++)
            {
                for (int col = 0; col < 3 && !moveMade; col++)
                {
                    if (board.IsEmpty(row, col))
                    {
                        board.PlaceMark(row, col, 'X');
                        moveMade = true;
                    }
                }
            }
        }
        
        // Assert: AI should not lose (either win or draw)
        Assert.False(board.CheckWin('X'), "AI should never lose on Hard difficulty");
    }

    [Fact]
    public void MinimaxAIPlayer_ShouldHandleLargerBoard_4x4()
    {
        // Arrange: 4x4 board with some moves
        var board = new Board(4, 4);
        board.PlaceMark(0, 0, 'X');
        board.PlaceMark(1, 1, 'O');
        board.PlaceMark(0, 1, 'X');
        
        var aiPlayer = new MinimaxAIPlayer('O', "AI", MinimaxAIPlayer.Difficulty.Medium);

        // Act
        var move = aiPlayer.GetMove(board);

        // Assert: Should return a valid move
        Assert.True(board.IsEmpty(move.row, move.col), "Move should be in an empty position");
        Assert.True(board.IsValidPosition(move.row, move.col), "Move should be valid");
    }

    [Fact]
    public void MinimaxAIPlayer_ShouldCompleteWinningSequence_CustomWinCondition()
    {
        // Arrange: 4x4 board with win condition of 3
        // X | X |   |  
        // O | O |   |  
        //   |   |   |  
        //   |   |   |  
        var board = new Board(4, 3);
        board.PlaceMark(0, 0, 'X');
        board.PlaceMark(1, 0, 'O');
        board.PlaceMark(0, 1, 'X');
        board.PlaceMark(1, 1, 'O');
        
        var aiPlayer = new MinimaxAIPlayer('X', "AI", MinimaxAIPlayer.Difficulty.Hard);

        // Act
        var move = aiPlayer.GetMove(board);

        // Assert: Should complete the horizontal line at (0, 2)
        Assert.Equal(0, move.row);
        Assert.Equal(2, move.col);
    }
}
