# Advanced Tic-Tac-Toe

A flexible, extensible Tic-Tac-Toe game written in C# (.NET 8.0) supporting various board sizes, win conditions, and game modes.

## Features

- **Traditional 3x3 game** with standard rules
- **Resizable boards** from 3x3 up to 10x10
- **Adjustable win condition** (N in a row, where N = 3-7)
- **Multiple game modes:**
  - Human vs Human
  - Human vs Computer
  - Computer vs Computer

## Requirements

- .NET 8.0 SDK or later

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

1. Select a game mode from the main menu (1-4)
2. Choose board size (3-10, default is 3)
3. Choose win condition (3-7, default is 3)
4. For Human vs Human or Human vs Computer modes, enter player name(s)
5. Follow the on-screen prompts to make moves
6. Enter row and column numbers (0-based) when it's your turn

## Project Structure

- **Board.cs** - Manages the game board, move validation, and win detection
- **Player.cs** - Abstract base class for all player types
- **HumanPlayer.cs** - Human player implementation with console input
- **ComputerPlayer.cs** - Computer player with random move strategy
- **GameController.cs** - Controls game flow and manages game state
- **Program.cs** - Main entry point with game mode selection

## Future Enhancements

- 3D board support (stacked grids with cross-layer win conditions)
- Network multiplayer support
- Advanced AI strategies (minimax, alpha-beta pruning)
- GUI implementation
- Save/load game state

## License

This project is part of a learning exercise following the plan outlined in ProjectPlan.md.
