Project: Advanced Tic-Tac-Toe (2D and 3D)
Purpose: Design a flexible, extensible tic-tac-toe game in C# with the following features:
  - Traditional 3x3 grid game for two players (human or computer)
  - Support for resizable grids (up to 10x10) with adjustable win condition (N in a row, N = 3-7)
  - Extend to 3D stacked grids with win conditions across, within, and between layers
  - Game modes: human vs human, human vs computer, computer vs computer

Major Steps:
1. Project Structure & Core Classes
   - Game controller/class for board, player, and game logic management
   - Abstract 'Player' class with 'HumanPlayer' and 'ComputerPlayer' implementations
   - Board data structure (2D/3D arrays), win condition logic

2. UI Implementation (Support both Console and GUI)
   - Print/display the board grid
   - Accept and validate player input
   - Show status and prompts

3. Game Logic
   - Turn-based play for two sides
   - Move validation and update logic
   - Win/draw detection (supporting all board sizes and dimensions)
   - Switch player logic

4. AI Support
   - Simple computer moves (random/heuristic)
   - Interface for more advanced AI later

5. Extensibility
   - Parameterize board size, win condition, and layers
   - Modular design for future enhancements

6. Game Modes
   - Select and launch desired mode at startup:
     - Human vs. Human (network enabled)
     - Human vs. Computer
     - Computer vs. Computer

Kickoff Implementation:
- Generate initial GameController class to instantiate board (2D/3D), manage players, and run game loop.
- Define Board structure for flexible grid size and extensible win checking.
- Create stub Player classes for human/computer input, with sample move selection logic.
- Scaffold main program for selection of play mode.

Instructions:
- Use .NET 8.0 features for robust data structures and user interaction.
- Organize code using best practices for readability and maintainability.
- Include XML doc comments for all public methods.
