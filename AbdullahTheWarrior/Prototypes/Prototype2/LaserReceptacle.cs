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
        private const int MinLength = 5;
        private static readonly Vector2 MaxLengthFactor = new Vector2(0.3f, 0.5f); // 30% map width, 50% map height

        // X,Y; width, height
        public static Tuple<int, int, int, int> FindLaserLocation(ArrayMap<bool> map)
        {
            var random = new Random();
            var isHorizontal = random.Next(100) <= 50;
            var maxLength = isHorizontal ? MaxLengthFactor.X * map.Width : MaxLengthFactor.Y *  map.Height;
            int halfLength = (int)Math.Round(maxLength / 2);

            while (true) {
                var x = random.Next(map.Width);
                var y = random.Next(map.Height);
                var endX = x;
                var endY = y;

                Vector2 size = Vector2.Zero;

                if (isHorizontal) {
                    endX = GetLastFreeX(map, x - halfLength, x + halfLength, y);
                } else {
                    endY = GetLastFreeY(map, x, y - halfLength, y + halfLength);
                }

                if (endX < x) {
                    var temp = x;
                    x = endX;
                    endX = x;
                }

                if (endY < y) {
                    var temp = y;
                    y = endY;
                    endY = y;
                }

                var dx = Math.Abs(endX - x);
                var dy = Math.Abs(endY - y);

                if ((dx >= MinLength || dy >= MinLength) && 
                    (dx <= (int)(map.Width * MaxLengthFactor.X)) &&
                    (dy <= (int)(map.Height * MaxLengthFactor.Y))) {
                        return new Tuple<int, int, int, int>(x, y, endX, endY);
                    }
            }
        }

        private static int GetLastFreeX(ArrayMap<bool> map, int startX, int endX, int y)
        {
            if (startX < 0) {
                startX = 0;
            }

            if (endX > map.Width - 1) {
                endX = map.Width - 1;
            }

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

        private static int GetLastFreeY(ArrayMap<bool> map, int startY, int endY, int x)
        {
            // Swap if needed
            if (startY > endY) {
                var temp = startY;
                startY = endY;
                endY = temp;
            }

            if (startY < 0) {
                startY = 0;
            }

            if (endY > map.Height - 1) {
                endY = map.Height - 1;
            }

            for (var y = startY; y <= endY; y++) {
                if (map[x, y] == true) {
                    return y;
                }
            }

            return endY;
        }
    }
}