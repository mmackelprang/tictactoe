using Terminal.Gui;

namespace TicTacToe;

/// <summary>
/// Provides a Terminal GUI interface for the Tic-Tac-Toe game.
/// </summary>
public class GuiInterface
{
    private Board _board;
    private Player _player1;
    private Player _player2;
    private Player _currentPlayer;
    private Window? _mainWindow;
    private Button[,] _buttons;
    private Label? _statusLabel;
    private Label? _turnLabel;
    private bool _gameOver;

    /// <summary>
    /// Initializes a new instance of the GuiInterface class.
    /// </summary>
    /// <param name="player1">The first player.</param>
    /// <param name="player2">The second player.</param>
    /// <param name="boardSize">The size of the board (default is 3).</param>
    /// <param name="winCondition">The number of consecutive marks needed to win (default is 3).</param>
    public GuiInterface(Player player1, Player player2, int boardSize = 3, int winCondition = 3)
    {
        _board = new Board(boardSize, winCondition);
        _player1 = player1;
        _player2 = player2;
        _currentPlayer = player1;
        _buttons = new Button[boardSize, boardSize];
        _gameOver = false;
    }

    /// <summary>
    /// Starts the GUI and runs the game.
    /// </summary>
    public void Run()
    {
        Application.Init();

        try
        {
            // Create main window
            _mainWindow = new Window("Tic-Tac-Toe")
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            // Create status label
            _statusLabel = new Label($"Board: {_board.Size}x{_board.Size} | Win: {_board.WinCondition} in a row")
            {
                X = 1,
                Y = 1
            };
            _mainWindow.Add(_statusLabel);

            // Create turn label
            _turnLabel = new Label($"{_currentPlayer.Name}'s turn ({_currentPlayer.Mark})")
            {
                X = 1,
                Y = 2
            };
            _mainWindow.Add(_turnLabel);

            // Create board buttons
            int buttonSize = 5;
            int startX = 2;
            int startY = 4;

            for (int row = 0; row < _board.Size; row++)
            {
                for (int col = 0; col < _board.Size; col++)
                {
                    int r = row; // Capture for lambda
                    int c = col; // Capture for lambda
                    
                    _buttons[row, col] = new Button("   ")
                    {
                        X = startX + col * (buttonSize + 1),
                        Y = startY + row * 2,
                        Width = buttonSize,
                        Height = 1
                    };
                    
                    _buttons[row, col].Clicked += () => OnCellClicked(r, c);
                    _mainWindow.Add(_buttons[row, col]);
                }
            }

            // Create quit button
            var quitButton = new Button("Quit")
            {
                X = Pos.Center(),
                Y = startY + _board.Size * 2 + 2
            };
            quitButton.Clicked += () => Application.RequestStop();
            _mainWindow.Add(quitButton);

            // Create new game button
            var newGameButton = new Button("New Game")
            {
                X = Pos.Center(),
                Y = startY + _board.Size * 2 + 4
            };
            newGameButton.Clicked += OnNewGame;
            _mainWindow.Add(newGameButton);

            Application.Top.Add(_mainWindow);

            // If the current player is a computer, make its move
            if (_currentPlayer is ComputerPlayer)
            {
                Application.MainLoop.Invoke(() => MakeComputerMove());
            }

            Application.Run();
        }
        finally
        {
            Application.Shutdown();
        }
    }

    /// <summary>
    /// Handles cell click events.
    /// </summary>
    private void OnCellClicked(int row, int col)
    {
        if (_gameOver)
            return;

        // Only allow human players to click
        if (_currentPlayer is ComputerPlayer)
            return;

        if (!_board.IsEmpty(row, col))
            return;

        MakeMove(row, col);
    }

    /// <summary>
    /// Makes a move at the specified position.
    /// </summary>
    private void MakeMove(int row, int col)
    {
        if (!_board.PlaceMark(row, col, _currentPlayer.Mark))
            return;

        // Update button text
        _buttons[row, col].Text = $" {_currentPlayer.Mark} ";

        // Check for win
        if (_board.CheckWin(_currentPlayer.Mark))
        {
            _gameOver = true;
            if (_turnLabel != null)
                _turnLabel.Text = $"🎉 {_currentPlayer.Name} ({_currentPlayer.Mark}) wins!";
            return;
        }

        // Check for draw
        if (_board.IsFull())
        {
            _gameOver = true;
            if (_turnLabel != null)
                _turnLabel.Text = "🤝 It's a draw!";
            return;
        }

        // Switch player
        SwitchPlayer();
        if (_turnLabel != null)
            _turnLabel.Text = $"{_currentPlayer.Name}'s turn ({_currentPlayer.Mark})";

        // If the new current player is a computer, make its move
        if (_currentPlayer is ComputerPlayer)
        {
            Application.MainLoop.Invoke(() => MakeComputerMove());
        }
    }

    /// <summary>
    /// Makes a computer player's move.
    /// </summary>
    private void MakeComputerMove()
    {
        if (_gameOver)
            return;

        Thread.Sleep(500); // Small delay for better UX
        var (row, col) = _currentPlayer.GetMove(_board);
        MakeMove(row, col);
    }

    /// <summary>
    /// Switches the current player to the other player.
    /// </summary>
    private void SwitchPlayer()
    {
        _currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;
    }

    /// <summary>
    /// Handles the new game button click.
    /// </summary>
    private void OnNewGame()
    {
        Application.RequestStop();
        Run();
    }
}
