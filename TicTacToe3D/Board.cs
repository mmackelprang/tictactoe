using System;
using System.Collections.Generic;
using System.Numerics;

namespace TicTacToe3D
{
    public class Board
    {
        public int Size { get; private set; }
        public int WinCount { get; private set; }
        public int[,,] Grid { get; private set; }

        public Board(int size, int winCount)
        {
            Size = size;
            WinCount = winCount;
            Grid = new int[Size, Size, Size];
        }

        public bool IsValidMove(int x, int y, int z)
        {
            if (x < 0 || x >= Size || y < 0 || y >= Size || z < 0 || z >= Size) return false;
            return Grid[x, y, z] == 0;
        }

        public void PlaceMove(int x, int y, int z, int player)
        {
            if (IsValidMove(x, y, z))
            {
                Grid[x, y, z] = player;
            }
        }

        public void ClearMove(int x, int y, int z)
        {
             if (x >= 0 && x < Size && y >= 0 && y < Size && z >= 0 && z < Size)
            {
                Grid[x, y, z] = 0;
            }
        }

        public bool CheckWin(int x, int y, int z, int player)
        {
             int[] range = { -1, 0, 1 };
             foreach (int dx in range)
                foreach (int dy in range)
                    foreach (int dz in range)
                    {
                        if (dx == 0 && dy == 0 && dz == 0) continue;
                        if (CheckLine(x, y, z, dx, dy, dz, player)) return true;
                    }
             return false;
        }

        private bool CheckLine(int x, int y, int z, int dx, int dy, int dz, int player)
        {
            if (!IsUniqueDirection(dx, dy, dz)) return false;
            int count = 1;
            count += CountConsecutive(x, y, z, dx, dy, dz, player);
            count += CountConsecutive(x, y, z, -dx, -dy, -dz, player);
            return count >= WinCount;
        }

        private int CountConsecutive(int x, int y, int z, int dx, int dy, int dz, int player)
        {
            int count = 0;
            int cx = x + dx, cy = y + dy, cz = z + dz;
            while (cx >= 0 && cx < Size && cy >= 0 && cy < Size && cz >= 0 && cz < Size && Grid[cx, cy, cz] == player)
            {
                count++;
                cx += dx; cy += dy; cz += dz;
            }
            return count;
        }

        private bool IsUniqueDirection(int dx, int dy, int dz) {
            if (dx > 0) return true;
            if (dx < 0) return false;
            if (dy > 0) return true;
            if (dy < 0) return false;
            return dz > 0;
        }

        public List<Vector3Int> GetEmptyCells()
        {
            var list = new List<Vector3Int>();
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                    for (int z = 0; z < Size; z++)
                        if (Grid[x, y, z] == 0) list.Add(new Vector3Int(x, y, z));
            return list;
        }
    }

    public struct Vector3Int { public int X, Y, Z; public Vector3Int(int x, int y, int z) { X=x; Y=y; Z=z; } }
}
