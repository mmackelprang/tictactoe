# Advanced Tic-Tac-Toe

A flexible, extensible Tic-Tac-Toe game written in C# (.NET 8.0) supporting various board sizes, win conditions, game modes, and both 2D and 3D gameplay.

## Features

- **Traditional 3x3 game** with standard rules
- **Resizable boards** from 3x3 up to 10x10
- **Adjustable win condition** (N in a row, where N = 3-7)
- **3D Mode** - Play with multiple stacked layers for an extra dimension of strategy
- **Smart AI** - Computer player can detect and block winning moves
- **Advanced AI** - Minimax algorithm with configurable difficulty levels
- **Multiple UI modes:**
  - Console Mode - Traditional text-based interface
  - GUI Mode - Terminal-based graphical interface using Terminal.Gui
  - Web Mode - Browser-based multiplayer interface with real-time updates
- **Multiple game modes:**
  - Human vs Human (local or online)
  - Human vs Computer
  - Computer vs Computer

## 3D Mode

When 3D mode is enabled, the game board consists of multiple layers (equal to the board size). For example, a 3x3 3D board has 3 layers of 3x3 grids stacked on top of each other.

### Winning in 3D Mode

Players can win by getting the required number of marks in a row:
- **Within a single layer**: Traditional horizontal, vertical, or diagonal wins
- **Across layers**: Vertical lines through the same position on different layers
- **Through layers diagonally**: Diagonal lines that traverse multiple layers

This creates a much more complex and strategic game with many more possible winning combinations!

## Requirements

- .NET 8.0 SDK or later

## AI Capabilities

The computer player uses a smart strategy with the following priority:
1. **Win** - Takes a winning move if available
2. **Block** - Blocks the opponent's winning move
3. **Random** - Makes a random move from available positions

This ensures competitive gameplay where the computer player is challenging but beatable.

## Building the Project

```bash
cd TicTacToe
dotnet build
```

## Running the Game

```bash
cd TicTacToe
dotnet run
```

## How to Play

1. **Web Application** (Recommended):
   ```bash
   cd TicTacToe.Web
   dotnet run
   ```
   Then open your browser to `https://localhost:5001` (or the URL shown in the console)
   - Enter your username and configure game settings (board size, win condition, AI difficulty, 3D mode)
   - Click "Enter Lobby" to join the game lobby
   - Either play against AI or invite other online players
   - In 3D mode, use the layer selector to choose which layer to place your mark on

2. **Console/GUI Mode**:
   ```bash
   cd TicTacToe
   dotnet run
   ```
   - Select UI mode (Console or GUI)
   - Select a game mode from the menu (1-3)
   - Choose board size (3-10, default is 3)
   - Choose win condition (3-7, default is 3)
   - For Human vs Human or Human vs Computer modes, enter player name(s)
   - **Console Mode**: Enter row and column numbers (0-based) when it's your turn
   - **GUI Mode**: Click on cells to make your move

## Project Structure

### Core Game Logic (TicTacToe project)
- **Board.cs** - Manages the game board (2D and 3D), move validation, and win detection
- **Player.cs** - Abstract base class for all player types
- **HumanPlayer.cs** - Human player implementation with console input
- **ComputerPlayer.cs** - Computer player with smart AI (detects and blocks wins)
- **MinimaxAIPlayer.cs** - Advanced AI using minimax algorithm with difficulty levels
- **GameController.cs** - Controls game flow and manages game state (console mode)
- **GuiInterface.cs** - Terminal-based GUI interface using Terminal.Gui
- **Program.cs** - Main entry point with UI and game mode selection

### Web Application (TicTacToe.Web project)
- **Hubs/GameHub.cs** - SignalR hub for real-time game communication
- **Services/** - Game management, lobby, and AI services
- **Models/** - Game state and session models
- **Components/Pages/** - Blazor pages for Home, Lobby, and Game
- **Program.cs** - Web application entry point and configuration

## Future Enhancements

### In Active Planning
See [ProjectPlan.md](ProjectPlan.md) for detailed implementation plans.

**Enhancements Under Consideration**:
- Enhanced 3D visualizations and animations
- Game replay and history
- Player profiles and statistics
- Tournament mode
- Save/load game state

## License

This project is part of a learning exercise following the plan outlined in ProjectPlan.md.
