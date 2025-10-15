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
   - ✅ Board data structure (3D arrays) with cross-layer win detection

2. UI Implementation (Support both Console and GUI)
   - ✅ Print/display the board grid
   - ✅ Accept and validate player input
   - ✅ Show status and prompts
   - ✅ Console Mode - Traditional text-based interface
   - ✅ GUI Mode - Terminal-based UI using Terminal.Gui
   - ✅ Web Application Mode - Browser-based UI with real-time updates
     - ✅ Create ASP.NET Core web project structure
     - ✅ Implement Blazor components for game board
     - ✅ Add SignalR for real-time communication
     - ✅ Design responsive layout for various screen sizes
     - ✅ Support for 3D mode with layer selection

3. Game Logic
   - ✅ Turn-based play for two sides
   - ✅ Move validation and update logic
   - ✅ Win/draw detection (supporting all board sizes and dimensions)
   - ✅ Switch player logic

4. AI Support
   - ✅ Smart computer moves with win detection and blocking
   - ✅ Priority-based strategy: Win > Block > Random
   - 🔄 Advanced AI algorithms (minimax, alpha-beta pruning)
     - Implement minimax algorithm with configurable depth
     - Add alpha-beta pruning for performance optimization
     - Create AI difficulty levels: Easy (random), Medium (minimax depth 3), Hard (full minimax)
     - Optimize for various board sizes and win conditions

5. Extensibility
   - ✅ Parameterize board size (3-10), win condition (3-7)
   - ✅ Modular design for future enhancements
   - ✅ 3D board support with multiple layers and cross-layer win conditions

6. Game Modes
   - ✅ Select and launch desired mode at startup:
     - ✅ Human vs. Human
     - ✅ Human vs. Computer (with smart AI)
     - ✅ Computer vs. Computer
   - 🔄 Network multiplayer modes:
     - ✅ Online Player vs. Online Player
     - ✅ Online Player vs. Computer AI
     - ✅ Game lobby and matchmaking system
     - ✅ Real-time game state synchronization
     - ✅ 3D mode support in multiplayer

Kickoff Implementation:
- ✅ Generate initial GameController class to instantiate board (2D), manage players, and run game loop.
- ✅ Define Board structure for flexible grid size and extensible win checking.
- ✅ Create Player classes for human/computer input with smart AI move selection logic.
- ✅ Scaffold main program for selection of UI mode and play mode.

Current Status (Completed):
- ✅ 2D Tic-Tac-Toe with flexible board sizes (3-10) and win conditions (3-7)
- ✅ 3D Tic-Tac-Toe with multiple layers and cross-layer win detection
- ✅ Smart AI that detects and blocks winning moves
- ✅ Advanced AI with Minimax algorithm and multiple difficulty levels
- ✅ Console and Terminal GUI modes
- ✅ Web-based multiplayer with real-time synchronization
- ✅ Multiple game modes (Human vs Human, Human vs Computer, Computer vs Computer)
- ✅ Game configuration persistence across sessions

Next Steps (Future Enhancements):
- Enhanced 3D visualizations and animations
- 3D AI support (minimax for 3D boards)
- Game replay and history
- Player profiles and statistics
- Save/load game state
- Native desktop GUI (WPF, Avalonia)
- Tournament mode

Network Multiplayer & Web Application (In Progress):
1. Web Application Infrastructure
   - Convert from Console application to ASP.NET Core web application
   - Implement web-based UI using Blazor Server or Razor Pages with SignalR
   - Replace Terminal.Gui with browser-based interactive UI
   - Add responsive design for desktop and mobile browsers

2. Network Multiplayer Features
   - Implement SignalR hub for real-time game communication
   - Create game lobby system for matchmaking
   - User connection management and presence tracking
   - Support for multiple concurrent games on server
   - Game room creation and joining functionality

3. Player Matchmaking System
   - Display list of connected users in lobby
   - Allow users to challenge other online players
   - Support for playing against computer AI
   - Game invitation and acceptance flow
   - Spectator mode for watching ongoing games

4. Advanced AI Implementation (Minimax & Alpha-Beta Pruning)
   - Implement Minimax algorithm for optimal move calculation
   - Add alpha-beta pruning optimization for faster decision making
   - Create difficulty levels (Easy: current random, Medium: minimax depth 3, Hard: full minimax with alpha-beta)
   - Optimize AI performance for larger board sizes
   - Add AI strategy evaluation metrics

Instructions:
- Use .NET 8.0 features for robust data structures and user interaction.
- Organize code using best practices for readability and maintainability.
- Include XML doc comments for all public methods.

---

## Detailed Implementation Plan for Network Multiplayer & Advanced AI

### Phase 1: Web Application Infrastructure

**Objective**: Convert the console application to a web-based application with real-time capabilities.

**Technical Stack**:
- ASP.NET Core 8.0 Web Application
- Blazor Server for interactive UI (or Razor Pages with JavaScript)
- SignalR for real-time bidirectional communication
- Bootstrap or modern CSS framework for responsive design

**Implementation Steps**:
1. Create new ASP.NET Core Web Application project
2. Set up SignalR hub for game communication
3. Port existing game logic (Board, Player, GameController) to be web-compatible
4. Create web-based game board UI component
5. Implement client-side interaction handlers
6. Add session management and user tracking

**File Structure**:
```
TicTacToe.Web/
├── Hubs/
│   └── GameHub.cs              # SignalR hub for real-time communication
├── Services/
│   ├── GameService.cs          # Manages multiple game instances
│   ├── LobbyService.cs         # Manages connected users and matchmaking
│   └── AIService.cs            # Provides AI move calculations
├── Models/
│   ├── GameSession.cs          # Represents an active game
│   ├── ConnectedUser.cs        # Represents a connected user
│   └── GameState.cs            # Serializable game state for sync
├── Pages/ or Components/
│   ├── Index.razor             # Landing page
│   ├── Lobby.razor             # Game lobby with user list
│   └── Game.razor              # Active game board
└── wwwroot/
    ├── css/                    # Styles
    ├── js/                     # Client-side scripts
    └── images/                 # Assets
```

### Phase 2: Network Multiplayer Features

**Game Lobby System**:
- Display all connected users with status (Available, In Game, etc.)
- Show active games and allow spectating
- Implement game invitation system
- Handle user connections and disconnections gracefully

**SignalR Hub Methods**:
```csharp
// Server to Client
- OnUserConnected(string userId, string username)
- OnUserDisconnected(string userId)
- OnGameInvitation(string fromUserId, string gameId)
- OnGameStarted(GameState gameState)
- OnMoveMade(int row, int col, char mark)
- OnGameEnded(string winnerId, string reason)
- OnLobbyUpdated(List<ConnectedUser> users)

// Client to Server
- JoinLobby(string username)
- LeaveLobby()
- InvitePlayer(string targetUserId)
- AcceptInvitation(string gameId)
- DeclineInvitation(string gameId)
- StartGameWithAI(int difficulty)
- MakeMove(string gameId, int row, int col)
- LeaveGame(string gameId)
```

**Game Session Management**:
- Support multiple concurrent games on the server
- Maintain game state in memory (or Redis for scalability)
- Handle player timeouts and disconnections
- Implement reconnection logic for dropped connections
- Persist game history (optional)

**Matchmaking Logic**:
```
1. User joins lobby → Receives list of available players
2. User selects opponent or chooses AI
3. If opponent selected:
   a. Send invitation to opponent
   b. Wait for acceptance/decline
   c. On acceptance, create game session
4. If AI selected:
   a. Create game session immediately
   b. Assign AI player to game
5. Start game and sync initial state to both clients
6. Handle moves via SignalR
7. Validate moves server-side
8. Broadcast move results to both players
9. Check win/draw conditions
10. End game and return players to lobby
```

### Phase 3: Advanced AI Implementation

**Minimax Algorithm**:
```csharp
public class MinimaxAIPlayer : Player
{
    private readonly int _maxDepth;
    private readonly bool _useAlphaBeta;
    
    public MinimaxAIPlayer(char mark, string name, int maxDepth, bool useAlphaBeta) 
        : base(mark, name)
    {
        _maxDepth = maxDepth;
        _useAlphaBeta = useAlphaBeta;
    }
    
    public override (int row, int col) GetMove(Board board)
    {
        if (_useAlphaBeta)
            return GetBestMoveWithAlphaBeta(board);
        else
            return GetBestMoveWithMinimax(board);
    }
    
    private (int row, int col) GetBestMoveWithMinimax(Board board)
    {
        // Implementation details below
    }
    
    private (int row, int col) GetBestMoveWithAlphaBeta(Board board)
    {
        // Implementation with alpha-beta pruning
    }
    
    private int Minimax(Board board, int depth, bool isMaximizing)
    {
        // Check terminal states
        if (board.CheckWin(Mark)) return 10 - depth;
        if (board.CheckWin(OpponentMark)) return depth - 10;
        if (board.IsFull()) return 0;
        if (depth >= _maxDepth) return EvaluatePosition(board);
        
        // Recursive minimax logic
    }
    
    private int MinimaxWithAlphaBeta(Board board, int depth, int alpha, int beta, bool isMaximizing)
    {
        // Similar to Minimax but with pruning
    }
    
    private int EvaluatePosition(Board board)
    {
        // Heuristic evaluation for non-terminal states
        // Count potential winning lines, central control, etc.
    }
}
```

**AI Difficulty Levels**:
- **Easy**: Current implementation (Win > Block > Random)
- **Medium**: Minimax with depth limit of 3-4 moves
- **Hard**: Full minimax with alpha-beta pruning (optimal play)
- **Adaptive**: Adjusts difficulty based on player performance (future)

**Performance Optimizations**:
- Implement move ordering (check center and corners first)
- Use transposition tables to cache evaluated positions
- Limit search depth for larger boards (5x5+)
- Consider iterative deepening for time-limited moves
- Pre-compute symmetrical positions to reduce search space

**AI Strategy Considerations**:
- For 3x3 boards: Perfect play is achievable
- For larger boards: Use depth-limited search with heuristics
- Heuristic evaluation: Center control, line potential, blocking opportunities
- Opening book: Pre-defined strong opening moves
- Endgame optimization: Exact calculation when few moves remain

### Phase 4: Integration and Testing

**Testing Requirements**:
1. Unit tests for AI algorithms
   - Minimax returns optimal moves
   - Alpha-beta pruning produces same results as minimax
   - Performance benchmarks for various board sizes

2. Integration tests for web components
   - SignalR hub communication
   - Game state synchronization
   - User connection/disconnection handling

3. End-to-end tests
   - Complete game flow: lobby → game → completion
   - Multiple concurrent games
   - AI vs Human gameplay
   - Human vs Human over network

4. Load testing
   - Server capacity with many concurrent games
   - SignalR connection limits
   - Response time under load

**Deployment Considerations**:
- Configure CORS for SignalR
- Set up HTTPS for production
- Configure connection timeouts
- Implement rate limiting
- Add logging and monitoring
- Consider scalability (load balancing, Redis backplane for SignalR)

### Phase 5: User Experience Enhancements

**UI/UX Features**:
- Animated piece placement
- Sound effects for moves
- Game timer/move timer
- Chat functionality between players
- Player profiles and statistics
- Elo rating system (optional)
- Game replay functionality
- Mobile-responsive design
- Dark mode support

**Accessibility**:
- Keyboard navigation support
- Screen reader compatibility
- High contrast mode
- Adjustable text sizes
- ARIA labels for game elements

---

## Success Criteria

**Network Multiplayer**:
- ✅ Users can connect to web application from browser
- ✅ Lobby displays all connected users
- ✅ Users can invite other players to games
- ✅ Games synchronize in real-time
- ✅ Multiple concurrent games supported
- ✅ Users can play against AI or other humans
- ✅ 3D mode available in multiplayer games
- ✅ Game configuration persists when returning to lobby

**Advanced AI**:
- ✅ Minimax algorithm implemented and working
- ✅ Alpha-beta pruning optimization functional
- ✅ Multiple difficulty levels available
- ✅ AI makes optimal moves on Hard difficulty
- ✅ Performance acceptable for standard board sizes (3x3 to 5x5)

**Quality**:
- ✅ No regressions in existing console/GUI functionality
- ✅ Comprehensive test coverage
- ✅ Documentation updated
- ✅ Clean, maintainable code structure
