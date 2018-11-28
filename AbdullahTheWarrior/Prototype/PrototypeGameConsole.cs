using System;
using System.Collections.Generic;
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
        private readonly Monster player = new Monster('@', Color.White, 40, 5, 3);
        private readonly Random random = new Random();
        private readonly List<Monster> monsters = new List<Monster>();
        private readonly List<Vector2> walls = new List<Vector2>();

        // TODO: use an entity with a position component
        private int playerX = 0;
        private int playerY = 0;

        public PrototypeGameConsole(int width, int height) : base(width, height)
        {
            playerX = width / 2;
            playerY = height / 3;

            this.GenerateWalls();
            this.GenerateMonsters();
        }

        private void GenerateWalls()
        {
            for (var x = 0; x < this.Width; x++)
            {
                this.walls.Add(new Vector2(x, 0));
                this.walls.Add(new Vector2(x, this.Height - 1));
            }

            for (var y = 0; y < this.Height; y++)
            {
                this.walls.Add(new Vector2(0, y));
                this.walls.Add(new Vector2(this.Width - 1, y));
            }

            for (var y = 15; y <= 20; y++) {
                for (var x = 35; x <= 45; x++) {
                    this.walls.Add(new Vector2(x, y));
                }
            }
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
            // One day, I will do better. One day, I will efficiently draw only what changed!
            for (var y = 0; y < this.Height; y++)
            {
                for (var x = 0; x < this.Width; x++)
                {
                    this.DrawCharacter(x, y, '.', DarkGrey);
                }
            }

            foreach (var wall in this.walls)
            {
                this.DrawCharacter(wall.X, wall.Y, '#', Color.DarkGray);
            }

            foreach (var monster in this.monsters)
            {
                this.DrawCharacter(monster.X, monster.Y, monster.Character, monster.Color);
            }

            this.DrawCharacter(playerX, playerY, player.Character, player.Color);
        }

        private void GenerateMonsters()
        {
            var numMonsters = random.Next(10, 15);
            while (this.monsters.Count < numMonsters)
            {
                var spot = this.FindEmptySpot();
                var monster = Monster.Create("Brigand");
                monster.X = (int)spot.X;
                monster.Y = (int)spot.Y;
                this.monsters.Add(monster);
            }
        }

        private Vector2 FindEmptySpot()
        {
            int targetX = 0;
            int targetY = 0;
            
            do 
            {
                targetX = random.Next(0, this.Width);
                targetY = random.Next(0, this.Height);
            } while (!this.IsWalkable(targetX, targetY));

            return new Vector2(targetX, targetY);
        }

        private bool IsWalkable(int x, int y)
        {
            foreach (var wall in this.walls)
            {
                if (wall.X == x && wall.Y == y)
                {
                    return false;
                }
            }

            foreach (var monster in this.monsters)
            {
                if (monster.X == x && monster.Y == y)
                {
                    return false;
                }
            }

            if (this.playerX == x && this.playerY == y)
            {
                return false;
            }

            return true;
        }

        private void DrawCharacter(float x, float y, char character, Color color)
        {
            var intX = (int)x;
            var intY = (int)y;
            // TODO: we should probably cache Cell instances, I'm sure this will hit the GC hard.
            this.SetCellAppearance(intX, intY, new Cell() { Background = Color.Black, Foreground = color });
            this.SetGlyph(intX, intY, character);
        }

        private class Monster
        {
            public int CurrentHealth { get; }
            public int TotalHealth { get; }
            public int Strength { get; }
            public int Defense { get; }
            public Color Color { get; }
            public char Character { get; }
            public int X { get; set; }
            public int Y { get; set; }

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