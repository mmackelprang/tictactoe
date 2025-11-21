using System;
using Raylib_cs;

namespace TicTacToe3D
{
    class Program
    {
        enum AppState
        {
            Menu,
            Playing
        }

        static AppState currentState = AppState.Menu;
        static Game? game;
        static Renderer? renderer;

        // Menu Settings
        static int selectedBoardSize = 4;
        static int selectedWinCount = 4;

        static void Main(string[] args)
        {
            Raylib.InitWindow(1280, 720, "3D Tic-Tac-Toe");
            Raylib.SetTargetFPS(60);

            while (!Raylib.WindowShouldClose())
            {
                if (currentState == AppState.Menu)
                {
                    UpdateMenu();
                    DrawMenu();
                }
                else if (currentState == AppState.Playing)
                {
                    if (game != null && renderer != null)
                    {
                        renderer.UpdateCamera();
                        renderer.HandleInput(game);
                        game.Update();
                        renderer.Draw(game);

                        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
                        {
                            currentState = AppState.Menu;
                        }
                    }
                }
            }

            Raylib.CloseWindow();
        }

        static void UpdateMenu()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.Up)) selectedBoardSize++;
            if (Raylib.IsKeyPressed(KeyboardKey.Down)) selectedBoardSize--;
            
            if (Raylib.IsKeyPressed(KeyboardKey.Right)) selectedWinCount++;
            if (Raylib.IsKeyPressed(KeyboardKey.Left)) selectedWinCount--;

            selectedBoardSize = Math.Clamp(selectedBoardSize, 3, 10);
            selectedWinCount = Math.Clamp(selectedWinCount, 3, selectedBoardSize);

            if (Raylib.IsKeyPressed(KeyboardKey.Enter))
            {
                StartGame();
            }
        }

        static void DrawMenu()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);

            int centerX = Raylib.GetScreenWidth() / 2;
            int centerY = Raylib.GetScreenHeight() / 2;

            DrawCenteredText("3D TIC-TAC-TOE", centerY - 150, 50, Color.DarkBlue);
            
            DrawCenteredText($"Board Size: {selectedBoardSize} (UP/DOWN to change)", centerY - 50, 30, Color.Black);
            DrawCenteredText($"Win Length: {selectedWinCount} (LEFT/RIGHT to change)", centerY, 30, Color.Black);
            
            DrawCenteredText("Press ENTER to Start", centerY + 100, 40, Color.Green);

            Raylib.EndDrawing();
        }

        static void DrawCenteredText(string text, int y, int fontSize, Color color)
        {
            int width = Raylib.MeasureText(text, fontSize);
            Raylib.DrawText(text, (Raylib.GetScreenWidth() - width) / 2, y, fontSize, color);
        }

        static void StartGame()
        {
            game = new Game(selectedBoardSize, selectedWinCount);
            renderer = new Renderer(selectedBoardSize);
            currentState = AppState.Playing;
        }
    }
}
