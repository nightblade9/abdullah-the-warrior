using System;
using System.Collections.Generic;
using System.Linq;
using DeenGames.AbdullahTheWarrior.Ecs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;

namespace DeenGames.AbdullahTheWarrior.Prototype
{
    public class PrototypeGameConsole : SadConsole.Console
    {
        private readonly Color DarkGrey = Color.FromNonPremultiplied(64,64,64,255);
        private readonly Entity player = new Entity('@', Color.White, 40, 5, 3);
        private readonly Random random = new Random();
        private readonly List<Entity> monsters = new List<Entity>();
        private readonly List<Vector2> walls = new List<Vector2>();

        public PrototypeGameConsole(int width, int height) : base(width, height)
        {
            player.X = width / 2;
            player.Y = height / 3;

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
            var destinationX = this.player.X;
            var destinationY = this.player.Y;
            
            if ((Global.KeyboardState.IsKeyPressed(Keys.W) || Global.KeyboardState.IsKeyPressed(Keys.Up)))
            {
                destinationY -= 1;
            }
            else if ((Global.KeyboardState.IsKeyPressed(Keys.S) || Global.KeyboardState.IsKeyPressed(Keys.Down)))
            {
                destinationY += 1;
            }

            if ((Global.KeyboardState.IsKeyPressed(Keys.A) || Global.KeyboardState.IsKeyPressed(Keys.Left)))
            {
                destinationX -= 1;
            }
            else if ((Global.KeyboardState.IsKeyPressed(Keys.D) || Global.KeyboardState.IsKeyPressed(Keys.Right)))
            {
                destinationX += 1;
            }
            
            if (this.IsWalkable(destinationX, destinationY))
            {
                this.player.X = destinationX;
                this.player.Y = destinationY;
            }
            else if (this.GetMonsterAt(destinationX, destinationY) != null)
            {
                var monster = this.GetMonsterAt(destinationX, destinationY);
                System.Console.WriteLine($"FIGHT: {monster.Character} {monster.CurrentHealth}/{monster.TotalHealth}");
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

            this.DrawCharacter(player.X, player.Y, player.Character, player.Color);
        }

        private void GenerateMonsters()
        {
            var numMonsters = random.Next(10, 15);
            while (this.monsters.Count < numMonsters)
            {
                var spot = this.FindEmptySpot();
                var monster = Entity.CreateFromTemplate("Brigand");
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

        private Entity GetMonsterAt(int x, int y)
        {
            return this.monsters.SingleOrDefault(m => m.X == x && m.Y == y);
        }

        private bool IsWalkable(int x, int y)
        {
            if (this.walls.Any(w => w.X == x && w.Y == y))
            {
                return false;
            }

            if (this.GetMonsterAt(x, y) != null)
            {
                return false;
            }

            if (this.player.X == x && this.player.Y == y)
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
    }
}