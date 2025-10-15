Project: Advanced Tic-Tac-Toe (2D and 3D)
Purpose: Design a flexible, extensible tic-tac-toe game in C# with the following features:
  - Traditional 3x3 grid game for two players (human or computer)
  - Support for resizable grids (up to 10x10) with adjustable win condition (N in a row, N = 3-7)
  - Extend to 3D stacked grids with win conditions across, within, and between layers
  - Game modes: human vs human, human vs computer, computer vs computer

Major Steps:
1. Project Structure & Core Classes
   - ✅ Game controller/class for board, player, and game logic management
   - ✅ Abstract 'Player' class with 'HumanPlayer' and 'ComputerPlayer' implementations
   - ✅ Board data structure (2D arrays), win condition logic
   - Board data structure (3D arrays) - Future enhancement

2. UI Implementation (Support both Console and GUI)
   - ✅ Print/display the board grid
   - ✅ Accept and validate player input
   - ✅ Show status and prompts
   - ✅ Console Mode - Traditional text-based interface
   - ✅ GUI Mode - Terminal-based UI using Terminal.Gui

3. Game Logic
   - ✅ Turn-based play for two sides
   - ✅ Move validation and update logic
   - ✅ Win/draw detection (supporting all board sizes and dimensions)
   - ✅ Switch player logic

4. AI Support
   - ✅ Smart computer moves with win detection and blocking
   - ✅ Priority-based strategy: Win > Block > Random
   - Interface for more advanced AI later (minimax, alpha-beta pruning)

5. Extensibility
   - ✅ Parameterize board size (3-10), win condition (3-7)
   - ✅ Modular design for future enhancements
   - Future: Support for 3D layers

6. Game Modes
   - ✅ Select and launch desired mode at startup:
     - ✅ Human vs. Human
     - ✅ Human vs. Computer (with smart AI)
     - ✅ Computer vs. Computer
     - Future: Network enabled multiplayer

Kickoff Implementation:
- ✅ Generate initial GameController class to instantiate board (2D), manage players, and run game loop.
- ✅ Define Board structure for flexible grid size and extensible win checking.
- ✅ Create Player classes for human/computer input with smart AI move selection logic.
- ✅ Scaffold main program for selection of UI mode and play mode.

Current Status (Completed):
- ✅ 2D Tic-Tac-Toe with flexible board sizes (3-10) and win conditions (3-7)
- ✅ Smart AI that detects and blocks winning moves
- ✅ Console and Terminal GUI modes
- ✅ Multiple game modes (Human vs Human, Human vs Computer, Computer vs Computer)

Next Steps (Future Enhancements):
- 3D board support with stacked grids
- Network multiplayer
- More advanced AI (minimax, alpha-beta pruning)
- Native desktop GUI (WPF, Avalonia)
- Save/load game state

Instructions:
- Use .NET 8.0 features for robust data structures and user interaction.
- Organize code using best practices for readability and maintainability.
- Include XML doc comments for all public methods.
