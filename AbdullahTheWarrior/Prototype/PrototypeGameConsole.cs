using System;
using DeenGames.AbdullahTheWarrior.Ecs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;

namespace DeenGames.AbdullahTheWarrior.Prototype
{
    public class PrototypeGameConsole : SadConsole.Console
    {
        private readonly Color DarkGrey = Color.FromNonPremultiplied(64,64,64,255);
        // Words cannot express my dismay. And amusement.
        private Monster player = new Monster('@', Color.White, 40, 5, 3);

        // TODO: use an entity with a position component
        private int playerX = 0;
        private int playerY = 0;

        public PrototypeGameConsole(int width, int height) : base(width, height)
        {
            playerX = width / 2;
            playerY = height / 3;

            this.GenerateMonsters();
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
                        this.DrawCharacter(x, y, '#', Color.Gray);
                    }
                    else if (x >= 35 && x <= 45 && y >= 15 && y <= 20)
                    {
                        this.DrawCharacter(x, y, '#', Color.Gray);
                    }
                    else
                    {
                        this.DrawCharacter(x, y, '.', DarkGrey);
                    }
                }
            }

            this.DrawCharacter(playerX, playerY, player.Character, player.Color);
        }

        private void GenerateMonsters()
        {
        }

        private bool IsWalkable(int x, int y)
        {
            // Poor man's data structures: visual representation *is* our data.
            return this.GetGlyph(x, y) == '.';
        }

        private void DrawCharacter(int x, int y, char character, Color color)
        {
            // TODO: we should probably cache Cell instances, I'm sure this will hit the GC hard.
            this.SetCellAppearance(x, y, new Cell() { Background = Color.Black, Foreground = color });
            this.SetGlyph(x, y, character);
        }

        private class Monster
        {
            public int CurrentHealth { get; }
            public int TotalHealth { get; }
            public int Strength { get; }
            public int Defense { get; }
            public Color Color { get; }
            public char Character { get; }

            // This code makes me cry.
            public static Monster Create(string name)
            {
                if (name == "Brigand")
                {
                    return new Monster('b', Color.Red, 20, 4, 1);
                }
                else
                {
                    throw new InvalidOperationException($"Not sure how to create a {name} monster");
                }
            }
            
            public Monster(char character, Color color, int health, int strength, int defense)
            {
                this.Character = character;
                this.Color = color;
                this.CurrentHealth = health;
                this.TotalHealth = health;
                this.Strength = strength;
                this.Defense = defense;
            }
        }
    }
}