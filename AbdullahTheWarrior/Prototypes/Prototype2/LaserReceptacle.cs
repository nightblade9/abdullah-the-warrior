using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace DeenGames.AbdullahTheWarrior.Prototypes.Prototype2
{
    /// <summary>
    /// A receptacle for a laser. Two of these, facing each other, make up a laser.
    // (You can also have just one of these firing endlessly.) Also contains static methods, like location finding.
    /// </summary>
    class LaserReceptacle
    {
        public int X {get; private set;}
        public int Y {get; private set;}

        private const int MinLength = 5;

        // Lasers fire every turn. Does this fire on odd or even turns?
        private bool firesOnAlternateTurns = false;
        private static readonly Vector2 MaxLengthFactor = new Vector2(0.3f, 0.5f); // 30% map width, 50% map height
        public Direction Direction {get; private set;}

        public LaserReceptacle(int x, int y, Direction direction)
        {
            this.X = x;
            this.Y = y;
            this.Direction = direction;
        }

        public static Tuple<int, int, int, int> FindLaserLocation(ArrayMap<bool> map)
        {
            var maxLength = MaxLengthFactor.X * map.Width;
            int halfLength = (int)Math.Round(maxLength / 2);

            while (true) {
                // TODO: prune these so they're not [0, w] but [0 + halfLength, w - halfLength]
                var x = PrototypeGameConsole.GlobalRandom.Next(map.Width);
                var y = PrototypeGameConsole.GlobalRandom.Next(map.Height);
                
                var endX = x;

                // Keep looking if the starting location is solid
                if (map[x, y] == true) {
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