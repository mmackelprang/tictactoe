using TicTacToe;
using Xunit;

namespace TicTacToe.Tests;

/// <summary>
/// Integration tests for AI gameplay scenarios.
/// </summary>
public class AIIntegrationTests
{
    [Fact]
    public void AIvsAI_HardDifficulty_ShouldAlwaysEndInDraw_3x3()
    {
        // Arrange: Two Hard AI players
        var board = new Board(3, 3);
        var player1 = new MinimaxAIPlayer('X', "AI-1", MinimaxAIPlayer.Difficulty.Hard);
        var player2 = new MinimaxAIPlayer('O', "AI-2", MinimaxAIPlayer.Difficulty.Hard);
        
        Player currentPlayer = player1;

        // Act: Play a complete game
        while (!board.IsFull() && !board.CheckWin('X') && !board.CheckWin('O'))
        {
            var move = currentPlayer.GetMove(board);
            board.PlaceMark(move.row, move.col, currentPlayer.Mark);
            
            currentPlayer = currentPlayer == player1 ? player2 : player1;
        }

        // Assert: Game should end in a draw (neither wins)
        Assert.False(board.CheckWin('X'), "Hard AI vs Hard AI should not have a winner");
        Assert.False(board.CheckWin('O'), "Hard AI vs Hard AI should not have a winner");
        Assert.True(board.IsFull(), "Board should be full");
    }

    [Fact]
    public void AIvsAI_DifferentDifficulties_HardShouldNeverLose_3x3()
    {
        // Arrange: Hard AI vs Easy AI
        var board = new Board(3, 3);
        var hardAI = new MinimaxAIPlayer('X', "Hard-AI", MinimaxAIPlayer.Difficulty.Hard);
        var easyAI = new ComputerPlayer('O', "Easy-AI");
        
        Player currentPlayer = hardAI;

        // Act: Play a complete game
        while (!board.IsFull() && !board.CheckWin('X') && !board.CheckWin('O'))
        {
            var move = currentPlayer.GetMove(board);
            board.PlaceMark(move.row, move.col, currentPlayer.Mark);
            
            currentPlayer = currentPlayer == hardAI ? easyAI : hardAI;
        }

        // Assert: Easy AI should never win against Hard AI
        Assert.False(board.CheckWin('O'), "Easy AI should not win against Hard AI");
        Assert.True(board.CheckWin('X') || board.IsFull(), "Hard AI should win or draw");
    }

    [Fact]
    public void ComputerPlayer_ShouldWinAgainstRandomPlay()
    {
        // Arrange
        var board = new Board(3, 3);
        var computerPlayer = new ComputerPlayer('X', "Computer");
        var random = new Random(42); // Fixed seed for reproducibility

        // Act: Play a game where opponent makes random moves
        bool isComputerTurn = true;
        while (!board.IsFull() && !board.CheckWin('X') && !board.CheckWin('O'))
        {
            if (isComputerTurn)
            {
                var move = computerPlayer.GetMove(board);
                board.PlaceMark(move.row, move.col, 'X');
            }
            else
            {
                // Random opponent move
                var emptyPositions = new List<(int row, int col)>();
                for (int row = 0; row < board.Size; row++)
                {
                    for (int col = 0; col < board.Size; col++)
                    {
                        if (board.IsEmpty(row, col))
                            emptyPositions.Add((row, col));
                    }
                }
                
                if (emptyPositions.Count > 0)
                {
                    var randomMove = emptyPositions[random.Next(emptyPositions.Count)];
                    board.PlaceMark(randomMove.row, randomMove.col, 'O');
                }
            }
            
            isComputerTurn = !isComputerTurn;
        }

        // Assert: Computer should not lose (win or draw)
        Assert.False(board.CheckWin('O'), "Computer should not lose to random play");
    }

    [Fact]
    public void MinimaxAI_ShouldHandleNearlyFullBoard()
    {
        // Arrange: Nearly full board with one winning move
        // X | O | X
        // O | X | O
        // O |   | X
        var board = new Board(3, 3);
        board.PlaceMark(0, 0, 'X');
        board.PlaceMark(0, 1, 'O');
        board.PlaceMark(0, 2, 'X');
        board.PlaceMark(1, 0, 'O');
        board.PlaceMark(1, 1, 'X');
        board.PlaceMark(1, 2, 'O');
        board.PlaceMark(2, 0, 'O');
        board.PlaceMark(2, 2, 'X');
        
        var aiPlayer = new MinimaxAIPlayer('X', "AI", MinimaxAIPlayer.Difficulty.Hard);

        // Act
        var move = aiPlayer.GetMove(board);

        // Assert: Should win at (2, 1)
        Assert.Equal(2, move.row);
        Assert.Equal(1, move.col);
        
        board.PlaceMark(move.row, move.col, 'X');
        Assert.True(board.CheckWin('X'), "AI should win after this move");
    }

    [Fact]
    public void MultipleGames_AIConsistency_ShouldMakeSameOptimalMovesInSamePosition()
    {
        // Arrange: Same board state
        var board1 = new Board(3, 3);
        var board2 = new Board(3, 3);
        
        // Place same moves on both boards
        board1.PlaceMark(0, 0, 'X');
        board1.PlaceMark(1, 1, 'O');
        board2.PlaceMark(0, 0, 'X');
        board2.PlaceMark(1, 1, 'O');
        
        var ai1 = new MinimaxAIPlayer('X', "AI-1", MinimaxAIPlayer.Difficulty.Hard);
        var ai2 = new MinimaxAIPlayer('X', "AI-2", MinimaxAIPlayer.Difficulty.Hard);

        // Act
        var move1 = ai1.GetMove(board1);
        var move2 = ai2.GetMove(board2);

        // Assert: Should make the same move (deterministic)
        Assert.Equal(move1.row, move2.row);
        Assert.Equal(move1.col, move2.col);
    }
}
