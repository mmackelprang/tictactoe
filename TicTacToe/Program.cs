using TicTacToe;

/// <summary>
/// Main entry point for the Tic-Tac-Toe game.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== TIC-TAC-TOE ===");
            Console.WriteLine("\nSelect Game Mode:");
            Console.WriteLine("1. Human vs Human");
            Console.WriteLine("2. Human vs Computer");
            Console.WriteLine("3. Computer vs Computer");
            Console.WriteLine("4. Exit");
            Console.Write("\nEnter your choice (1-4): ");

            string? choice = Console.ReadLine();

            if (choice == "4")
            {
                Console.WriteLine("Thanks for playing!");
                break;
            }

            // Get board configuration
            int boardSize = GetBoardSize();
            int winCondition = GetWinCondition(boardSize);

            Player player1, player2;

            switch (choice)
            {
                case "1":
                    Console.Write("Enter Player 1 name: ");
                    string? name1 = Console.ReadLine() ?? "Player 1";
                    Console.Write("Enter Player 2 name: ");
                    string? name2 = Console.ReadLine() ?? "Player 2";
                    player1 = new HumanPlayer('X', name1);
                    player2 = new HumanPlayer('O', name2);
                    break;

                case "2":
                    Console.Write("Enter your name: ");
                    string? humanName = Console.ReadLine() ?? "Player";
                    player1 = new HumanPlayer('X', humanName);
                    player2 = new ComputerPlayer('O', "Computer");
                    break;

                case "3":
                    player1 = new ComputerPlayer('X', "Computer 1");
                    player2 = new ComputerPlayer('O', "Computer 2");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Press any key to continue...");
                    Console.ReadKey();
                    continue;
            }

            var game = new GameController(player1, player2, boardSize, winCondition);
            game.PlayGame();

            Console.WriteLine("\nWould you like to play again? (y/n): ");
            string? playAgain = Console.ReadLine();
            if (playAgain?.ToLower() != "y")
            {
                Console.WriteLine("Thanks for playing!");
                break;
            }
        }
    }

    /// <summary>
    /// Prompts the user for the board size.
    /// </summary>
    /// <returns>The board size (3-10).</returns>
    static int GetBoardSize()
    {
        while (true)
        {
            Console.Write("\nEnter board size (3-10, default 3): ");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
                return 3;

            if (int.TryParse(input, out int size) && size >= 3 && size <= 10)
                return size;

            Console.WriteLine("Invalid size. Please enter a number between 3 and 10.");
        }
    }

    /// <summary>
    /// Prompts the user for the win condition.
    /// </summary>
    /// <param name="boardSize">The size of the board.</param>
    /// <returns>The win condition (3-7 and not exceeding board size).</returns>
    static int GetWinCondition(int boardSize)
    {
        while (true)
        {
            Console.Write($"Enter win condition (3-{Math.Min(7, boardSize)}, default 3): ");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
                return 3;

            if (int.TryParse(input, out int condition) && 
                condition >= 3 && 
                condition <= 7 && 
                condition <= boardSize)
                return condition;

            Console.WriteLine($"Invalid win condition. Please enter a number between 3 and {Math.Min(7, boardSize)}.");
        }
    }
}
