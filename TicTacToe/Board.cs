namespace TicTacToe;

/// <summary>
/// Represents the game board for Tic-Tac-Toe with flexible dimensions and win conditions.
/// Supports both 2D and 3D (layered) boards.
/// </summary>
public class Board
{
    private readonly char[,,] _grid;
    private readonly int _size;
    private readonly int _winCondition;
    private readonly bool _is3D;
    private readonly int _layers;

    /// <summary>
    /// Gets the size of the board (number of rows and columns).
    /// </summary>
    public int Size => _size;

    /// <summary>
    /// Gets the number of consecutive marks needed to win.
    /// </summary>
    public int WinCondition => _winCondition;

    /// <summary>
    /// Gets whether this is a 3D board.
    /// </summary>
    public bool Is3D => _is3D;

    /// <summary>
    /// Gets the number of layers (depth) in the board. For 2D boards, this is 1.
    /// </summary>
    public int Layers => _layers;

    /// <summary>
    /// Initializes a new instance of the Board class.
    /// </summary>
    /// <param name="size">The size of the board (default is 3).</param>
    /// <param name="winCondition">The number of consecutive marks needed to win (default is 3).</param>
    /// <param name="is3D">Whether to create a 3D board with multiple layers (default is false).</param>
    public Board(int size = 3, int winCondition = 3, bool is3D = false)
    {
        if (size < 3 || size > 10)
            throw new ArgumentException("Board size must be between 3 and 10.", nameof(size));
        if (winCondition < 3 || winCondition > 7 || winCondition > size)
            throw new ArgumentException("Win condition must be between 3 and 7 and not exceed board size.", nameof(winCondition));

        _size = size;
        _winCondition = winCondition;
        _is3D = is3D;
        _layers = is3D ? size : 1;
        _grid = new char[_layers, size, size];
        InitializeBoard();
    }

    /// <summary>
    /// Initializes the board with empty cells.
    /// </summary>
    private void InitializeBoard()
    {
        for (int layer = 0; layer < _layers; layer++)
        {
            for (int row = 0; row < _size; row++)
            {
                for (int col = 0; col < _size; col++)
                {
                    _grid[layer, row, col] = ' ';
                }
            }
        }
    }

    /// <summary>
    /// Places a mark on the board at the specified position.
    /// </summary>
    /// <param name="row">The row index (0-based).</param>
    /// <param name="col">The column index (0-based).</param>
    /// <param name="mark">The mark to place (typically 'X' or 'O').</param>
    /// <param name="layer">The layer index (0-based, default is 0 for 2D).</param>
    /// <returns>True if the move was successful, false otherwise.</returns>
    public bool PlaceMark(int row, int col, char mark, int layer = 0)
    {
        if (!IsValidPosition(row, col, layer) || !IsEmpty(row, col, layer))
            return false;

        _grid[layer, row, col] = mark;
        return true;
    }

    /// <summary>
    /// Clears a position on the board (used for simulating moves).
    /// </summary>
    /// <param name="row">The row index (0-based).</param>
    /// <param name="col">The column index (0-based).</param>
    /// <param name="layer">The layer index (0-based, default is 0 for 2D).</param>
    internal void ClearPosition(int row, int col, int layer = 0)
    {
        if (IsValidPosition(row, col, layer))
        {
            _grid[layer, row, col] = ' ';
        }
    }

    /// <summary>
    /// Checks if a position is valid (within board boundaries).
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="col">The column index.</param>
    /// <param name="layer">The layer index (default is 0 for 2D).</param>
    /// <returns>True if the position is valid, false otherwise.</returns>
    public bool IsValidPosition(int row, int col, int layer = 0)
    {
        return layer >= 0 && layer < _layers && row >= 0 && row < _size && col >= 0 && col < _size;
    }

    /// <summary>
    /// Checks if a cell is empty.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="col">The column index.</param>
    /// <param name="layer">The layer index (default is 0 for 2D).</param>
    /// <returns>True if the cell is empty, false otherwise.</returns>
    public bool IsEmpty(int row, int col, int layer = 0)
    {
        return IsValidPosition(row, col, layer) && _grid[layer, row, col] == ' ';
    }

    /// <summary>
    /// Gets the mark at the specified position.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <param name="col">The column index.</param>
    /// <param name="layer">The layer index (default is 0 for 2D).</param>
    /// <returns>The mark at the position, or space if empty or invalid.</returns>
    public char GetMark(int row, int col, int layer = 0)
    {
        return IsValidPosition(row, col, layer) ? _grid[layer, row, col] : ' ';
    }

    /// <summary>
    /// Checks if the board is full (no empty cells).
    /// </summary>
    /// <returns>True if the board is full, false otherwise.</returns>
    public bool IsFull()
    {
        for (int layer = 0; layer < _layers; layer++)
        {
            for (int row = 0; row < _size; row++)
            {
                for (int col = 0; col < _size; col++)
                {
                    if (_grid[layer, row, col] == ' ')
                        return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Checks if there is a winner on the board.
    /// </summary>
    /// <param name="mark">The mark to check for a win.</param>
    /// <returns>True if the specified mark has won, false otherwise.</returns>
    public bool CheckWin(char mark)
    {
        // Check all layers individually (2D wins within each layer)
        for (int layer = 0; layer < _layers; layer++)
        {
            // Check rows
            for (int row = 0; row < _size; row++)
            {
                for (int col = 0; col <= _size - _winCondition; col++)
                {
                    if (CheckLine(layer, row, col, 0, 0, 1, mark))
                        return true;
                }
            }

            // Check columns
            for (int col = 0; col < _size; col++)
            {
                for (int row = 0; row <= _size - _winCondition; row++)
                {
                    if (CheckLine(layer, row, col, 0, 1, 0, mark))
                        return true;
                }
            }

            // Check diagonal (top-left to bottom-right)
            for (int row = 0; row <= _size - _winCondition; row++)
            {
                for (int col = 0; col <= _size - _winCondition; col++)
                {
                    if (CheckLine(layer, row, col, 0, 1, 1, mark))
                        return true;
                }
            }

            // Check anti-diagonal (top-right to bottom-left)
            for (int row = 0; row <= _size - _winCondition; row++)
            {
                for (int col = _winCondition - 1; col < _size; col++)
                {
                    if (CheckLine(layer, row, col, 0, 1, -1, mark))
                        return true;
                }
            }
        }

        // For 3D boards, check cross-layer wins
        if (_is3D && _layers >= _winCondition)
        {
            // Check vertical lines through layers (same row, same col, different layers)
            for (int row = 0; row < _size; row++)
            {
                for (int col = 0; col < _size; col++)
                {
                    for (int layer = 0; layer <= _layers - _winCondition; layer++)
                    {
                        if (CheckLine(layer, row, col, 1, 0, 0, mark))
                            return true;
                    }
                }
            }

            // Check diagonal through layers (layer, row, col all change)
            for (int layer = 0; layer <= _layers - _winCondition; layer++)
            {
                for (int row = 0; row <= _size - _winCondition; row++)
                {
                    for (int col = 0; col <= _size - _winCondition; col++)
                    {
                        if (CheckLine(layer, row, col, 1, 1, 1, mark))
                            return true;
                    }
                }
            }

            // Check anti-diagonal through layers (layer and row increase, col decreases)
            for (int layer = 0; layer <= _layers - _winCondition; layer++)
            {
                for (int row = 0; row <= _size - _winCondition; row++)
                {
                    for (int col = _winCondition - 1; col < _size; col++)
                    {
                        if (CheckLine(layer, row, col, 1, 1, -1, mark))
                            return true;
                    }
                }
            }

            // Check diagonal through layers (layer and col increase, row decreases)
            for (int layer = 0; layer <= _layers - _winCondition; layer++)
            {
                for (int row = _winCondition - 1; row < _size; row++)
                {
                    for (int col = 0; col <= _size - _winCondition; col++)
                    {
                        if (CheckLine(layer, row, col, 1, -1, 1, mark))
                            return true;
                    }
                }
            }

            // Check anti-diagonal through layers (layer increases, row and col decrease)
            for (int layer = 0; layer <= _layers - _winCondition; layer++)
            {
                for (int row = _winCondition - 1; row < _size; row++)
                {
                    for (int col = _winCondition - 1; col < _size; col++)
                    {
                        if (CheckLine(layer, row, col, 1, -1, -1, mark))
                            return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Checks a line for consecutive marks.
    /// </summary>
    /// <param name="startLayer">The starting layer.</param>
    /// <param name="startRow">The starting row.</param>
    /// <param name="startCol">The starting column.</param>
    /// <param name="deltaLayer">The layer direction (0 or 1).</param>
    /// <param name="deltaRow">The row direction (0, 1, or -1).</param>
    /// <param name="deltaCol">The column direction (0, 1, or -1).</param>
    /// <param name="mark">The mark to check for.</param>
    /// <returns>True if the line contains the required consecutive marks.</returns>
    private bool CheckLine(int startLayer, int startRow, int startCol, int deltaLayer, int deltaRow, int deltaCol, char mark)
    {
        for (int i = 0; i < _winCondition; i++)
        {
            int layer = startLayer + i * deltaLayer;
            int row = startRow + i * deltaRow;
            int col = startCol + i * deltaCol;
            if (_grid[layer, row, col] != mark)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Returns a string representation of the board.
    /// </summary>
    /// <returns>A formatted string showing the current board state.</returns>
    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        
        for (int layer = 0; layer < _layers; layer++)
        {
            if (_is3D)
            {
                sb.AppendLine();
                sb.AppendLine($"=== Layer {layer} ===");
            }
            
            sb.AppendLine();
            
            // Print column numbers
            sb.Append("   ");
            for (int col = 0; col < _size; col++)
            {
                sb.Append($" {col} ");
                if (col < _size - 1)
                    sb.Append(" ");
            }
            sb.AppendLine();

            for (int row = 0; row < _size; row++)
            {
                // Print row number
                sb.Append($" {row} ");
                
                for (int col = 0; col < _size; col++)
                {
                    sb.Append($" {_grid[layer, row, col]} ");
                    if (col < _size - 1)
                        sb.Append("|");
                }
                sb.AppendLine();

                // Print separator line (except after last row)
                if (row < _size - 1)
                {
                    sb.Append("   ");
                    for (int col = 0; col < _size; col++)
                    {
                        sb.Append("---");
                        if (col < _size - 1)
                            sb.Append("+");
                    }
                    sb.AppendLine();
                }
            }
        }
        
        return sb.ToString();
    }
}
