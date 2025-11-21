using System;
using System.Numerics;
using Raylib_cs;

namespace TicTacToe3D
{
    public class Renderer
    {
        private const float CellSpacing = 2.0f;
        private const float CellSize = 1.5f;

        private Camera3D camera;
        private float cameraAngleX = MathF.PI / 4.0f;
        private float cameraAngleY = MathF.PI / 4.0f;
        private float cameraDistance = 25.0f;
        private Vector3 boardOffset;

        public Renderer(int boardSize)
        {
            // Setup Camera
            camera = new Camera3D();
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.FovY = 45.0f;
            camera.Projection = CameraProjection.Perspective;
            camera.Target = Vector3.Zero;

            // Offset to center board drawing at 0,0,0
            float offset = (boardSize - 1) * CellSpacing / 2.0f;
            boardOffset = new Vector3(offset, offset, offset);
        }

        public void UpdateCamera()
        {
            // Check for Right Mouse Button Drag
            if (Raylib.IsMouseButtonDown(MouseButton.Right))
            {
                Vector2 delta = Raylib.GetMouseDelta();
                float sensitivity = 0.01f;

                cameraAngleX -= delta.X * sensitivity;
                cameraAngleY += delta.Y * sensitivity;

                float limit = MathF.PI / 2.0f - 0.1f;
                cameraAngleY = Math.Clamp(cameraAngleY, -limit, limit);
            }

            // Zoom with Scroll Wheel
            float wheel = Raylib.GetMouseWheelMove();
            if (wheel != 0)
            {
                cameraDistance -= wheel * 2.0f;
                cameraDistance = Math.Clamp(cameraDistance, 10.0f, 50.0f);
            }

            // Convert Spherical Coordinates to Cartesian
            float horizDist = cameraDistance * MathF.Cos(cameraAngleY);
            camera.Position.Y = cameraDistance * MathF.Sin(cameraAngleY);
            camera.Position.X = horizDist * MathF.Sin(cameraAngleX);
            camera.Position.Z = horizDist * MathF.Cos(cameraAngleX);
        }

        public void Draw(Game game)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);

            Raylib.BeginMode3D(camera);

            // Draw Grid
            for (int x = 0; x < game.Board.Size; x++)
            {
                for (int y = 0; y < game.Board.Size; y++)
                {
                    for (int z = 0; z < game.Board.Size; z++)
                    {
                        Vector3 pos = new Vector3(x * CellSpacing, y * CellSpacing, z * CellSpacing) - boardOffset;
                        
                        Raylib.DrawCubeWires(pos, CellSize, CellSize, CellSize, Color.LightGray);

                        if (game.Board.Grid[x, y, z] == 1) // Human
                            Raylib.DrawSphere(pos, CellSize / 2.2f, Color.Red);
                        else if (game.Board.Grid[x, y, z] == 2) // AI
                            Raylib.DrawCube(pos, CellSize / 1.5f, CellSize / 1.5f, CellSize / 1.5f, Color.Blue);
                    }
                }
            }

            Raylib.EndMode3D();

            Raylib.DrawText("Right Click + Drag to Rotate", 10, 10, 20, Color.DarkGray);
            Raylib.DrawText("Scroll to Zoom", 10, 30, 20, Color.DarkGray);
            Raylib.DrawText(game.StatusMsg, 10, 60, 30, game.GameOver ? game.WinColor : Color.Black);
            Raylib.DrawText("Press ESC to return to Menu", 10, 90, 20, Color.DarkGray);

            Raylib.EndDrawing();
        }

        public void HandleInput(Game game)
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && game.PlayerTurn && !game.GameOver)
            {
                Ray ray = Raylib.GetScreenToWorldRay(Raylib.GetMousePosition(), camera);
                
                RayCollision collision = new RayCollision();
                collision.Distance = float.MaxValue;
                collision.Hit = false;

                int targetX = -1, targetY = -1, targetZ = -1;

                for (int x = 0; x < game.Board.Size; x++)
                {
                    for (int y = 0; y < game.Board.Size; y++)
                    {
                        for (int z = 0; z < game.Board.Size; z++)
                        {
                            if (game.Board.Grid[x, y, z] != 0) continue; 

                            Vector3 pos = new Vector3(x * CellSpacing, y * CellSpacing, z * CellSpacing) - boardOffset;
                            
                            BoundingBox box = new BoundingBox(
                                pos - new Vector3(CellSize/2), 
                                pos + new Vector3(CellSize/2)
                            );

                            RayCollision boxHit = Raylib.GetRayCollisionBox(ray, box);

                            if (boxHit.Hit && boxHit.Distance < collision.Distance)
                            {
                                collision = boxHit;
                                targetX = x; targetY = y; targetZ = z;
                            }
                        }
                    }
                }

                if (targetX != -1)
                {
                    game.HandlePlayerMove(targetX, targetY, targetZ);
                }
            }
        }
    }
}
