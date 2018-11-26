using System;
using DeenGames.EmanGems.Ecs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;

namespace DeenGames.EmanGems.Prototype
{
    public class PrototypeGameConsole : SadConsole.Console
    {
        // TODO: use an entity with a position component
        private int playerX = 0;
        private int playerY = 0;

        public PrototypeGameConsole(int width, int height) : base(width, height)
        {
            playerX = width / 2;
            playerY = height / 3;
        }

        public override void Update(System.TimeSpan delta)
        {
            this.ProcessPlayerInput();
            this.RedrawEverything();
        }

        private void ProcessPlayerInput()
        {
            var destination = new Vector2(this.playerX, this.playerY);
            
            if (Global.KeyboardState.IsKeyPressed(Keys.W) && IsWalkable(this.playerX, this.playerY - 1))
            {
                playerY -= 1;
            }
            else if (Global.KeyboardState.IsKeyPressed(Keys.S) && IsWalkable(this.playerX, this.playerY + 1))
            {
                playerY += 1;
            }

            if (Global.KeyboardState.IsKeyPressed(Keys.A) && IsWalkable(this.playerX - 1, this.playerY))
            {
                playerX -= 1;
            }
            else if (Global.KeyboardState.IsKeyPressed(Keys.D) && IsWalkable(this.playerX + 1, this.playerY))
            {
                playerX += 1;
            }
        }

        private void RedrawEverything()
        {
            for (var y = 0; y < this.Height; y++)
            {
                for (var x = 0; x < this.Width; x++)
                {
                    if (x == 0 || y == 0 || x == this.Width - 1 || y == this.Height - 1)
                    {
                        this.SetGlyph(x, y, '#');
                    }
                    else if (x >= 35 && x <= 45 && y >= 15 && y <= 20)
                    {
                        this.SetGlyph(x, y, '#');
                    }
                    else
                    {
                        this.SetGlyph(x, y, '.');
                    }
                }
            }

            this.SetGlyph(playerX, playerY, '@');
        }

        private bool IsWalkable(int x, int y)
        {
            return this.GetGlyph(x, y) == '.';
        }
    }
}