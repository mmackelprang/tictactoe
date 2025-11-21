# 3D Tic-Tac-Toe

A configurable 3D implementation of Tic-Tac-Toe built with C# and Raylib. Challenge an AI opponent on a 3D grid of varying sizes.

## How to Play

### Menu Controls
*   **UP / DOWN Arrows**: Adjust Board Size (from 3x3x3 up to 10x10x10).
*   **LEFT / RIGHT Arrows**: Adjust Win Length (number of consecutive marks needed to win).
*   **ENTER**: Start the game with the selected settings.

### In-Game Controls
*   **Left Click**: Place your marker (Red Sphere) on an empty cell.
*   **Right Click + Drag**: Orbit the camera around the board to get a better view.
*   **Mouse Wheel**: Zoom in and out.
*   **ESC**: End the current game and return to the Main Menu.

## Game Rules
*   The game is played on a 3D grid.
*   You are the **Red Sphere**. The AI is the **Blue Cube**.
*   Players take turns placing their markers in empty cells.
*   The first player to align the specified number of markers (Win Length) in any direction wins.
    *   Directions include horizontal, vertical, depth, and all 3D diagonals.

## Architecture

The project follows a modular architecture to separate rendering, logic, and data:

### 1. `Program.cs` (Entry Point)
*   Manages the application lifecycle and the main game loop.
*   Handles the **Application State** (switching between Menu and Playing modes).
*   Draws and updates the Main Menu.

### 2. `Board.cs` (Data Model)
*   Represents the 3D grid state (`int[,,]`).
*   Contains core logic for:
    *   Validating moves.
    *   Checking win conditions (scanning all possible lines in 3D space).
    *   Tracking empty cells.

### 3. `Game.cs` (Game Logic)
*   Manages the flow of a single game session.
*   Tracks whose turn it is and the game-over state.
*   **AI Logic**: Implements a basic AI that prioritizes winning, then blocking the player, then random moves.

### 4. `Renderer.cs` (Presentation)
*   Handles all 3D rendering using **Raylib**.
*   Manages the **Camera3D** (orbit and zoom functionality).
*   Handles Raycasting for mouse input (converting 2D screen clicks to 3D board coordinates).

## Technologies
*   **C#**: Core programming language.
*   **Raylib-cs**: C# bindings for Raylib, used for graphics and input handling.
*   **.NET**: Framework.

## Build & Run

1.  Navigate to the project directory:
    ```powershell
    cd TicTacToe3D
    ```
2.  Run the project:
    ```powershell
    dotnet run
    ```
