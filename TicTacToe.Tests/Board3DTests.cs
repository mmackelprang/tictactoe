using TicTacToe;
using Xunit;

namespace TicTacToe.Tests;

/// <summary>
/// Tests for 3D board functionality.
/// </summary>
public class Board3DTests
{
    [Fact]
    public void Board3D_Constructor_ShouldCreateMultipleLayers()
    {
        // Arrange & Act
        var board = new Board(3, 3, is3D: true);

        // Assert
        Assert.True(board.Is3D);
        Assert.Equal(3, board.Layers);
        Assert.Equal(3, board.Size);
    }

    [Fact]
    public void Board3D_PlaceMark_ShouldPlaceOnCorrectLayer()
    {
        // Arrange
        var board = new Board(3, 3, is3D: true);

        // Act
        board.PlaceMark(0, 0, 'X', layer: 0);
        board.PlaceMark(0, 0, 'O', layer: 1);
        board.PlaceMark(0, 0, 'X', layer: 2);

        // Assert
        Assert.Equal('X', board.GetMark(0, 0, 0));
        Assert.Equal('O', board.GetMark(0, 0, 1));
        Assert.Equal('X', board.GetMark(0, 0, 2));
    }

    [Fact]
    public void Board3D_CheckWin_VerticalThroughLayers_ShouldDetectWin()
    {
        // Arrange
        var board = new Board(3, 3, is3D: true);

        // Act: Place marks vertically through layers at position (1, 1)
        board.PlaceMark(1, 1, 'X', layer: 0);
        board.PlaceMark(1, 1, 'X', layer: 1);
        board.PlaceMark(1, 1, 'X', layer: 2);

        // Assert
        Assert.True(board.CheckWin('X'));
    }

    [Fact]
    public void Board3D_CheckWin_DiagonalThroughLayers_ShouldDetectWin()
    {
        // Arrange
        var board = new Board(3, 3, is3D: true);

        // Act: Place marks diagonally through layers (0,0,0) -> (1,1,1) -> (2,2,2)
        board.PlaceMark(0, 0, 'X', layer: 0);
        board.PlaceMark(1, 1, 'X', layer: 1);
        board.PlaceMark(2, 2, 'X', layer: 2);

        // Assert
        Assert.True(board.CheckWin('X'));
    }

    [Fact]
    public void Board3D_CheckWin_WithinSingleLayer_ShouldDetectWin()
    {
        // Arrange
        var board = new Board(3, 3, is3D: true);

        // Act: Place marks in a row on layer 1
        board.PlaceMark(0, 0, 'X', layer: 1);
        board.PlaceMark(0, 1, 'X', layer: 1);
        board.PlaceMark(0, 2, 'X', layer: 1);

        // Assert
        Assert.True(board.CheckWin('X'));
    }

    [Fact]
    public void Board3D_IsFull_ShouldCheckAllLayers()
    {
        // Arrange
        var board = new Board(3, 3, is3D: true);

        // Act: Fill all cells on all layers
        for (int layer = 0; layer < 3; layer++)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    board.PlaceMark(row, col, (row + col + layer) % 2 == 0 ? 'X' : 'O', layer);
                }
            }
        }

        // Assert
        Assert.True(board.IsFull());
    }

    [Fact]
    public void Board3D_IsFull_WithEmptyCellOnAnyLayer_ShouldReturnFalse()
    {
        // Arrange
        var board = new Board(3, 3, is3D: true);

        // Act: Fill most cells but leave one empty on layer 2
        for (int layer = 0; layer < 3; layer++)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (layer == 2 && row == 2 && col == 2)
                        continue; // Leave this cell empty
                    board.PlaceMark(row, col, 'X', layer);
                }
            }
        }

        // Assert
        Assert.False(board.IsFull());
    }

    [Fact]
    public void Board3D_IsEmpty_ShouldCheckCorrectLayer()
    {
        // Arrange
        var board = new Board(3, 3, is3D: true);
        board.PlaceMark(1, 1, 'X', layer: 1);

        // Assert
        Assert.True(board.IsEmpty(1, 1, layer: 0));
        Assert.False(board.IsEmpty(1, 1, layer: 1));
        Assert.True(board.IsEmpty(1, 1, layer: 2));
    }

    [Fact]
    public void Board3D_ToString_ShouldDisplayAllLayers()
    {
        // Arrange
        var board = new Board(3, 3, is3D: true);
        board.PlaceMark(0, 0, 'X', layer: 0);
        board.PlaceMark(1, 1, 'O', layer: 1);
        board.PlaceMark(2, 2, 'X', layer: 2);

        // Act
        var output = board.ToString();

        // Assert
        Assert.Contains("Layer 0", output);
        Assert.Contains("Layer 1", output);
        Assert.Contains("Layer 2", output);
    }

    [Fact]
    public void Board2D_BackwardCompatibility_ShouldStillWork()
    {
        // Arrange & Act
        var board = new Board(3, 3, is3D: false);

        // Assert
        Assert.False(board.Is3D);
        Assert.Equal(1, board.Layers);
        
        // Test basic 2D functionality still works
        board.PlaceMark(0, 0, 'X');
        board.PlaceMark(0, 1, 'O');
        board.PlaceMark(1, 1, 'X');
        
        Assert.Equal('X', board.GetMark(0, 0));
        Assert.Equal('O', board.GetMark(0, 1));
        Assert.Equal('X', board.GetMark(1, 1));
    }

    [Fact]
    public void Board3D_LargerBoard_ShouldSupportDifferentSizes()
    {
        // Arrange & Act
        var board = new Board(5, 4, is3D: true);

        // Assert
        Assert.True(board.Is3D);
        Assert.Equal(5, board.Layers);
        Assert.Equal(5, board.Size);
        Assert.Equal(4, board.WinCondition);
    }

    [Fact]
    public void Board3D_CheckWin_ComplexDiagonal_ShouldDetectWin()
    {
        // Arrange: 4x4x4 board with win condition 4
        var board = new Board(4, 4, is3D: true);

        // Act: Place marks in a 3D diagonal
        board.PlaceMark(0, 0, 'X', layer: 0);
        board.PlaceMark(1, 1, 'X', layer: 1);
        board.PlaceMark(2, 2, 'X', layer: 2);
        board.PlaceMark(3, 3, 'X', layer: 3);

        // Assert
        Assert.True(board.CheckWin('X'));
    }
}
