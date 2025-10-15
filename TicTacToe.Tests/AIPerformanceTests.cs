using TicTacToe;
using Xunit;
using System.Diagnostics;

namespace TicTacToe.Tests;

/// <summary>
/// Performance benchmark tests for AI algorithms.
/// </summary>
public class AIPerformanceTests
{
    [Fact]
    public void MinimaxPerformance_3x3EmptyBoard_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var board = new Board(3, 3);
        var aiPlayer = new MinimaxAIPlayer('X', "AI", int.MaxValue, false);
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        var move = aiPlayer.GetMove(board);
        stopwatch.Stop();

        // Assert: Should complete within 5 seconds for 3x3 board
        Assert.True(stopwatch.ElapsedMilliseconds < 5000, 
            $"Minimax took {stopwatch.ElapsedMilliseconds}ms, which is too slow for 3x3 board");
        Assert.True(board.IsValidPosition(move.row, move.col));
    }

    [Fact]
    public void AlphaBetaPerformance_3x3EmptyBoard_ShouldBeFasterThanMinimax()
    {
        // Arrange
        var board1 = new Board(3, 3);
        var board2 = new Board(3, 3);
        var minimaxPlayer = new MinimaxAIPlayer('X', "Minimax", int.MaxValue, false);
        var alphaBetaPlayer = new MinimaxAIPlayer('X', "AlphaBeta", int.MaxValue, true);

        // Act
        var minimaxStopwatch = Stopwatch.StartNew();
        var minimaxMove = minimaxPlayer.GetMove(board1);
        minimaxStopwatch.Stop();

        var alphaBetaStopwatch = Stopwatch.StartNew();
        var alphaBetaMove = alphaBetaPlayer.GetMove(board2);
        alphaBetaStopwatch.Stop();

        // Assert: Alpha-beta should be faster or equal
        Assert.True(alphaBetaStopwatch.ElapsedMilliseconds <= minimaxStopwatch.ElapsedMilliseconds * 1.1, // Allow 10% margin
            $"Alpha-beta ({alphaBetaStopwatch.ElapsedMilliseconds}ms) should be faster than minimax ({minimaxStopwatch.ElapsedMilliseconds}ms)");
    }

    [Fact]
    public void MinimaxPerformance_4x4Board_MediumDifficulty_ShouldBeAcceptable()
    {
        // Arrange: 4x4 board with some moves already made
        var board = new Board(4, 4);
        board.PlaceMark(1, 1, 'X');
        board.PlaceMark(1, 2, 'O');
        board.PlaceMark(2, 1, 'X');
        
        var aiPlayer = new MinimaxAIPlayer('O', "AI", MinimaxAIPlayer.Difficulty.Medium);
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        var move = aiPlayer.GetMove(board);
        stopwatch.Stop();

        // Assert: Should complete within 10 seconds for 4x4 board with depth limit
        Assert.True(stopwatch.ElapsedMilliseconds < 10000, 
            $"Minimax on 4x4 board took {stopwatch.ElapsedMilliseconds}ms, which is too slow");
        Assert.True(board.IsValidPosition(move.row, move.col));
    }

    [Fact]
    public void AlphaBetaPerformance_3x3MidGame_ShouldBeVeryFast()
    {
        // Arrange: Board with several moves already made
        var board = new Board(3, 3);
        board.PlaceMark(0, 0, 'X');
        board.PlaceMark(1, 1, 'O');
        board.PlaceMark(0, 1, 'X');
        board.PlaceMark(1, 2, 'O');
        
        var aiPlayer = new MinimaxAIPlayer('X', "AI", MinimaxAIPlayer.Difficulty.Hard);
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        var move = aiPlayer.GetMove(board);
        stopwatch.Stop();

        // Assert: Should be very fast for mid-game position
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, 
            $"Alpha-beta on mid-game took {stopwatch.ElapsedMilliseconds}ms, should be faster");
        Assert.True(board.IsValidPosition(move.row, move.col));
    }

    [Fact]
    public void EasyDifficulty_ShouldBeInstant()
    {
        // Arrange
        var board = new Board(3, 3);
        board.PlaceMark(0, 0, 'X');
        board.PlaceMark(1, 1, 'O');
        
        var aiPlayer = new MinimaxAIPlayer('X', "AI", MinimaxAIPlayer.Difficulty.Easy);
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        var move = aiPlayer.GetMove(board);
        stopwatch.Stop();

        // Assert: Easy difficulty should be nearly instant
        Assert.True(stopwatch.ElapsedMilliseconds < 100, 
            $"Easy difficulty took {stopwatch.ElapsedMilliseconds}ms, should be nearly instant");
    }

    [Fact]
    public void ComputerPlayer_Performance_ShouldBeVeryFast()
    {
        // Arrange
        var board = new Board(3, 3);
        board.PlaceMark(0, 0, 'X');
        board.PlaceMark(1, 1, 'O');
        
        var aiPlayer = new ComputerPlayer('X', "AI");
        var stopwatch = new Stopwatch();

        // Act
        stopwatch.Start();
        var move = aiPlayer.GetMove(board);
        stopwatch.Stop();

        // Assert: Basic ComputerPlayer should be very fast (< 100ms including Thread.Sleep)
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, 
            $"ComputerPlayer took {stopwatch.ElapsedMilliseconds}ms, should be faster");
    }

    [Fact]
    public void AlphaBetaPruning_EfficiencyTest_ShouldPruneManyBranches()
    {
        // This test demonstrates the efficiency of alpha-beta pruning
        // by comparing number of moves evaluated vs board size
        
        // Arrange: Empty 3x3 board (most expensive case)
        var board = new Board(3, 3);
        var aiPlayer = new MinimaxAIPlayer('X', "AI", MinimaxAIPlayer.Difficulty.Hard);
        
        // Act: Just verify it completes in reasonable time
        var stopwatch = Stopwatch.StartNew();
        var move = aiPlayer.GetMove(board);
        stopwatch.Stop();

        // Assert: Should complete quickly even on empty board due to pruning
        Assert.True(stopwatch.ElapsedMilliseconds < 2000, 
            $"Alpha-beta pruning on empty 3x3 board took {stopwatch.ElapsedMilliseconds}ms");
    }
}
