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
        private readonly Entity player = new Entity("You", '@', Palette.White, 40, 5, 3, 6, numberOfAttacks: 1);
        private readonly Random random = new Random();
        private readonly List<Entity> monsters = new List<Entity>();
        private readonly List<Vector2> walls = new List<Vector2>();
        private int playerTurnsLeftUntilMonsterTurns = 0;

        private readonly int mapHeight;
        private string latestMessage = "";

        public PrototypeGameConsole(int width, int height) : base(width, height)
        {
            this.mapHeight = height - 2;
            
            player.X = width / 2;
            player.Y = mapHeight / 3;
            this.playerTurnsLeftUntilMonsterTurns = player.NumberOfTurns;

            this.GenerateWalls();
            this.GenerateMonsters();

            this.RedrawEverything();

            EventBus.Instance.AddListener(GameEvent.EntityDeath, (e) => {
                if (e == player)
                {
                    this.latestMessage = "YOU DIE!!!";
                    this.player.Character = '%';
                    this.player.Color = Palette.DarkBrownPurple;
                    this.RedrawEverything();
                }
                else
                {
                    this.monsters.Remove(e as Entity);
                }
            });
        }

        private void GenerateWalls()
        {
            for (var x = 0; x < this.Width; x++)
            {
                this.walls.Add(new Vector2(x, 0));
                this.walls.Add(new Vector2(x, this.mapHeight - 1));
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
            bool playerPressedKey = this.ProcessPlayerInput();
            if (playerPressedKey)
            {
                this.playerTurnsLeftUntilMonsterTurns -= 1;
                if (this.playerTurnsLeftUntilMonsterTurns <= 0)
                {
                    this.playerTurnsLeftUntilMonsterTurns = player.NumberOfTurns;
                    this.ProcessMonsterTurns();
                }
                this.RedrawEverything();
            }
        }

        private void ProcessMonsterTurns()
        {
            foreach (var monster in this.monsters)
            {
                var distance = Math.Sqrt(Math.Pow(player.X - monster.X, 2) + Math.Pow(player.Y - monster.Y, 2));
                if (distance <= monster.VisionRange)
                {
                    // Process turn.

                    if (distance <= 1)
                    {
                        // ATTACK~!
                        var damage = AttackResolver.Attacks(monster, player);
                        this.latestMessage += $" {monster.Name} attacks for {damage} damage!";
                    }
                    else
                    {
                        // Move closer. Naively. Randomly.
                        var dx = player.X - monster.X;
                        var dy = player.Y - monster.Y;
                        var tryHorizontallyFirst = random.Next(0, 100) <= 50;
                        if (tryHorizontallyFirst && dx != 0)
                        {
                            this.TryToMove(monster, monster.X + Math.Sign(dx), monster.Y);
                        }
                        else
                        {
                            this.TryToMove(monster, monster.X, monster.Y + Math.Sign(dy));
                        }
                    }
                }
            }
        }

        private bool TryToMove(Entity entity, int targetX, int targetY)
        {
            // Assuming targetX/targetY are adjacent, or entity can fly/teleport, etc.
            if (this.IsWalkable(targetX, targetY))
            {
                entity.X = targetX;
                entity.Y = targetY;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ProcessPlayerInput()
        {            
            var destinationX = this.player.X;
            var destinationY = this.player.Y;
            var processedInput = false;
            
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
            
            if (this.TryToMove(player, destinationX, destinationY))
            {
                processedInput = true;
                this.latestMessage = "";
            }
            else if (this.GetMonsterAt(destinationX, destinationY) != null)
            {
                var monster = this.GetMonsterAt(destinationX, destinationY);
                processedInput = true;

                var totalDamage = 0;
                int attacksLeft = player.NumberOfAttacks;
                do
                {
                    attacksLeft -= 1;
                    var damage = AttackResolver.Attacks(player, monster);
                    totalDamage += damage;
                } while (attacksLeft > 0);

                var times = player.NumberOfAttacks <= 1 ? "" : $" {player.NumberOfAttacks}x";
                
                this.latestMessage = $"You hit {monster.Name}{times} for {totalDamage} damage!";
            }
            else if (Global.KeyboardState.IsKeyPressed(Keys.OemPeriod) || Global.KeyboardState.IsKeyPressed(Keys.Space))
            {
                // Skip turn
                processedInput = true;
            }

            if (player.CurrentHealth <= 0)
            {
                Environment.Exit(0);
            }

            return processedInput;
        }

        private void RedrawEverything()
        {
            // One day, I will do better. One day, I will efficiently draw only what changed!
            for (var y = 0; y < this.mapHeight; y++)
            {
                for (var x = 0; x < this.Width; x++)
                {
                    var colour = Palette.DarkGrey;
                    if (IsInPlayerFov(x, y))
                    {
                        colour = Palette.LightGrey;
                    }
                    this.DrawCharacter(x, y, '.', colour);
                }
            }

            foreach (var wall in this.walls)
            {
                var colour = Palette.DarkGrey;
                if (IsInPlayerFov((int)wall.X, (int)wall.Y))
                {
                    colour = Palette.LightGrey;
                }
                this.DrawCharacter(wall.X, wall.Y, '#', colour);
            }

            foreach (var monster in this.monsters)
            {
                if (IsInPlayerFov(monster.X, monster.Y))
                {
                    this.DrawCharacter(monster.X, monster.Y, monster.Character, monster.Color);
                }
            }

            this.DrawCharacter(player.X, player.Y, player.Character, player.Color);

            this.DrawLine(new Point(0, this.Height - 2), new Point(this.Width, this.Height - 2), null, Palette.DarkPurpleBrown, ' ');
            this.DrawLine(new Point(0, this.Height - 1), new Point(this.Width, this.Height - 1), null, Palette.DarkPurpleBrown, ' ');
            this.DrawHealthIndicators();
            this.Print(0, this.Height - 1, this.latestMessage, Palette.White);
        }

        private void DrawHealthIndicators()
        {
            string message = $"You: {player.CurrentHealth}/{player.TotalHealth}";
            
            foreach (var monster in this.monsters)
            {
                var distance = Math.Sqrt(Math.Pow(monster.X - player.X, 2) + Math.Pow(monster.Y - player.Y, 2));
                if (distance <= 1)
                {
                    // compact
                    message = $"{message} {monster.Character}: {monster.CurrentHealth}/{monster.TotalHealth}"; 
                }
            }

            this.Print(1, this.Height - 2, message, Palette.White);
        }

        private bool IsInPlayerFov(int x, int y)
        {
            // Doesn't use LoS calculations, just simple range check
            var distance = Math.Sqrt(Math.Pow(player.X - x, 2) + Math.Pow(player.Y - y, 2));
            return distance <= player.VisionRange;
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
            this.SetCellAppearance(intX, intY, new Cell() { Background = Palette.DarkestBlue, Foreground = color });
            this.SetGlyph(intX, intY, character);
        }
    }
}