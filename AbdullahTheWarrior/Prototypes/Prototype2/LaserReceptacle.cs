using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeenGames.AbdullahTheWarrior.Prototypes.Prototype2
{
    /// <summary>
    /// A receptacle for a laser. Two of these, facing each other, make up a laser.
    // (You can also have just one of these firing endlessly.) Also contains static methods, like location finding.
    /// </summary>
    class LaserReceptacle : AbstractEntity
    {
        private static readonly Vector2 MaxLengthFactor = new Vector2(0.3f, 0.5f); // 30% map width, 50% map height
        private const int MinLength = 5;

        public Direction Direction {get; private set;}

        public bool IsOn { get; private set; } = false;

        public IList<Vector2> Beams { get; private set; } = new List<Vector2>();

        public LaserReceptacle(int x, int y, Direction direction, bool isAlternating)
        : base(direction == Direction.Right ? '}' : '{', Palette.Brown)
        {
            this.X = x;
            this.Y = y;
            this.Direction = direction;
            if (isAlternating) { 
                this.IsOn = true;
            }
        }

        public static Tuple<int, int, int, int> FindLaserLocation(ArrayMap<bool> map, List<LaserReceptacle> lasers, List<AbstractEntity> walls, List<Entity> monsters)
        {
            var maxLength = MaxLengthFactor.X * map.Width;
            int halfLength = (int)Math.Round(maxLength / 2);

            while (true) {
                var x = PrototypeGameConsole.GlobalRandom.Next(halfLength, map.Width - halfLength);
                var y = PrototypeGameConsole.GlobalRandom.Next(halfLength, map.Height - halfLength);
                
                var endX = x;

                // Keep looking if the starting location is solid
                if (map[x, y] == true) {
                    continue;
                }

                if (lasers.Any(l => l.X == x && l.Y == y) || walls.Any(w => w.X == x && w.Y == y) || monsters.Any(m => m.X == x && m.Y == y))
                {
                    // This spot is already occupied
                    continue;
                }

                var startX = Math.Max(0, x - halfLength);
                var stopX = Math.Min(map.Width - 1, x + halfLength);

                // Check the end point
                if (map[startX, y] || map[stopX, y]) {
                    continue;
                }

                Vector2 size = Vector2.Zero;

                endX = GetLastFreeX(map, startX, endX, y);

                if (endX < startX) {
                    var temp = startX;
                    startX = endX;
                    endX = temp;
                }

                var dx = Math.Abs(endX - startX);
                if (dx >= MinLength && dx <= (int)(map.Width * MaxLengthFactor.X)) {
                    return new Tuple<int, int, int, int>(startX, y, endX, y);
                }
            }
        }

        public static Direction Invert(Direction source)
        {
            switch(source) {
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    throw new InvalidOperationException($"Not sure how to invert {source} direction");
            }
        }

        public void ProcessTurn()
        {
            this.IsOn = !this.IsOn;
            if (!this.IsOn) {
                this.Beams.Clear();
            }
        }

        public void Fire(List<LaserReceptacle> lasers, List<AbstractEntity> walls, List<Entity> monsters, int mapWidth, int mapHeight)
        {
            this.Beams.Clear();

            bool isFiring = true;
            Vector2 next = new Vector2(this.X, this.Y);
            Vector2 unitStep;
            switch (this.Direction) {
                case Direction.Left:
                    unitStep = new Vector2(-1, 0);
                    break;
                case Direction.Right:
                    unitStep = new Vector2(1, 0);
                    break;
                default:
                    throw new InvalidOperationException($"Not sure how to fire a laser {this.Direction}");
            }

            while (isFiring)
            {
                next += unitStep;
                var x = (int)next.X;
                var y = (int)next.Y;
                var addBeam = true;

                if (x < 0 || y < 0 || x > mapWidth || y >= mapHeight)
                {
                    isFiring = false;
                    break;
                }

                var wall = walls.SingleOrDefault(w => w.X == x && w.Y == y);
                if (wall != null) {
                    walls.Remove(wall);
                }
                
                // Harming the player is done in PrototypeGameConsole.cs; look for: player.Die()
                var monster = monsters.SingleOrDefault(m => m.X == x && m.Y == y);
                if (monster != null) {
                    // instant disintegration!
                    monster.Die();
                }

                // Should be a single laser but in some cases, we generate two on the same spot
                // eg. seed #285359147
                var laser = lasers.SingleOrDefault(l => l.X == x && l.Y == y);
                if (laser != null) {
                    if (laser.Direction == Invert(this.Direction))
                    {
                        addBeam = false;
                        isFiring = false;                        
                    }
                    else
                    {
                        // KABOOM!
                        // laser.Explode();
                    }
                }

                if (addBeam)
                {
                    this.Beams.Add(next);
                }
            }
        }

        private static int GetLastFreeX(ArrayMap<bool> map, int startX, int endX, int y)
        {
            // Swap if needed
            if (startX > endX) {
                var temp = startX;
                startX = endX;
                endX = temp;
            }

            for (var x = startX; x <= endX; x++) {
                if (map[x, y] == true) {
                    return x;
                }
            }

            return endX;
        }
    }
}