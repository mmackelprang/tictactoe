using System;
using System.Collections.Generic;
using Raylib_cs;

namespace TicTacToe3D
{
    public class Game
    {
        public Board Board { get; private set; }
        public bool PlayerTurn { get; private set; } = true;
        public bool GameOver { get; private set; } = false;
        public string StatusMsg { get; private set; } = "Your Turn (Click a box)";
        public Color WinColor { get; private set; } = Color.Green;

        public Game(int boardSize, int winCount)
        {
            Board = new Board(boardSize, winCount);
        }

        public void Update()
        {
            if (!PlayerTurn && !GameOver)
            {
                PerformAITurn();
            }
        }

        public void HandlePlayerMove(int x, int y, int z)
        {
            if (PlayerTurn && !GameOver && Board.IsValidMove(x, y, z))
            {
                ApplyMove(x, y, z, 1);
            }
        }

        private void PerformAITurn()
        {
            List<Vector3Int> empty = Board.GetEmptyCells();
            if (empty.Count == 0) return;

            Vector3Int bestMove = empty[new Random().Next(empty.Count)];
            
            // AI: Win -> Block -> Random
            foreach(var move in empty) if(SimulateMove(move.X, move.Y, move.Z, 2)) { bestMove = move; break; }
            if(Board.Grid[bestMove.X, bestMove.Y, bestMove.Z] == 0) {
                 foreach(var move in empty) if(SimulateMove(move.X, move.Y, move.Z, 1)) { bestMove = move; break; } 
            }

            ApplyMove(bestMove.X, bestMove.Y, bestMove.Z, 2);
        }

        private void ApplyMove(int x, int y, int z, int player)
        {
            Board.PlaceMove(x, y, z, player);
            if (Board.CheckWin(x, y, z, player))
            {
                GameOver = true;
                StatusMsg = (player == 1) ? "YOU WIN!" : "AI WINS!";
                WinColor = (player == 1) ? Color.Red : Color.Blue;
            }
            else
            {
                PlayerTurn = !PlayerTurn;
                StatusMsg = PlayerTurn ? "Your Turn" : "AI Thinking...";
            }
        }

        private bool SimulateMove(int x, int y, int z, int player)
        {
            Board.PlaceMove(x, y, z, player);
            bool win = Board.CheckWin(x, y, z, player);
            Board.ClearMove(x, y, z);
            return win;
        }
    }
}
