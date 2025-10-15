# Advanced Tic-Tac-Toe

A flexible, extensible Tic-Tac-Toe game written in C# (.NET 8.0) supporting various board sizes, win conditions, and game modes.

## Features

- **Traditional 3x3 game** with standard rules
- **Resizable boards** from 3x3 up to 10x10
- **Adjustable win condition** (N in a row, where N = 3-7)
- **Smart AI** - Computer player can detect and block winning moves
- **Multiple UI modes:**
  - Console Mode - Traditional text-based interface
  - GUI Mode - Terminal-based graphical interface using Terminal.Gui
- **Multiple game modes:**
  - Human vs Human
  - Human vs Computer
  - Computer vs Computer

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

1. Select UI mode (Console or GUI)
2. Select a game mode from the menu (1-3)
3. Choose board size (3-10, default is 3)
4. Choose win condition (3-7, default is 3)
5. For Human vs Human or Human vs Computer modes, enter player name(s)
6. **Console Mode**: Enter row and column numbers (0-based) when it's your turn
7. **GUI Mode**: Click on cells to make your move

## Project Structure

- **Board.cs** - Manages the game board, move validation, and win detection
- **Player.cs** - Abstract base class for all player types
- **HumanPlayer.cs** - Human player implementation with console input
- **ComputerPlayer.cs** - Computer player with smart AI (detects and blocks wins)
- **GameController.cs** - Controls game flow and manages game state (console mode)
- **GuiInterface.cs** - Terminal-based GUI interface using Terminal.Gui
- **Program.cs** - Main entry point with UI and game mode selection

## Future Enhancements

### In Active Planning
See [ProjectPlan.md](ProjectPlan.md) for detailed implementation plans.

**Network Multiplayer & Web Application**:
- Convert to ASP.NET Core web application with Blazor/SignalR
- Replace Console UI with browser-based interactive interface
- Implement game lobby for matchmaking with connected users
- Support multiple concurrent games on server
- Real-time game synchronization between players
- Game invitation and acceptance system
- Spectator mode for watching games

**Advanced AI (Minimax & Alpha-Beta Pruning)**:
- Implement minimax algorithm for optimal move calculation
- Add alpha-beta pruning for performance optimization
- Multiple difficulty levels (Easy, Medium, Hard)
- Optimized AI for various board sizes

### Future Considerations
- 3D board support (stacked grids with cross-layer win conditions)
- Native desktop GUI (WPF, WinForms, or Avalonia)
- Save/load game state
- Player profiles and statistics
- Game replay functionality

## License

This project is part of a learning exercise following the plan outlined in ProjectPlan.md.
