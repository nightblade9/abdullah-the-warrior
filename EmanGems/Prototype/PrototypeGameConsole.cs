using System;
using DeenGames.EmanGems.Ecs;
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
            playerY = height / 2;
        }

        public override void Update(System.TimeSpan delta)
        {
            this.ProcessPlayerInput();
            this.RedrawEverything();
        }

        private void ProcessPlayerInput()
        {
            if (Global.KeyboardState.IsKeyPressed(Keys.W))
            {
                this.playerY -= 1;
            }
            else if (Global.KeyboardState.IsKeyPressed(Keys.S))
            {
                this.playerY += 1;
            }

            if (Global.KeyboardState.IsKeyPressed(Keys.A))
            {
                this.playerX -= 1;
            }
            else if (Global.KeyboardState.IsKeyPressed(Keys.D))
            {
                this.playerX += 1;
                System.Console.WriteLine("!");
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
                    else
                    {
                        this.SetGlyph(x, y, '.');
                    }
                }
            }

            this.SetGlyph(playerX, playerY, '@');
        }
    }
}