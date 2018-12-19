using System;
using System.Collections.Generic;
using System.Linq;

namespace DeenGames.AbdullahTheWarrior.Prototype
{
    /// <summary>
    /// Small little class to handle Ghazi sword-skills drawing/input/etc.
    /// </summary>
    public class SwordSkillsManager
    {
        /// <summary>
        /// Dictionary of skill => list of steps. Each step is a rotation value (in degrees).
        /// eg. a step of zero means "keep going in the same direction." 90 means "rotate (clockwise) 90 degrees.
        /// </summary>
        private Dictionary<string, List<int>> SwordSkillSteps = new Dictionary<string, List<int>>()
        {
            { "L-Strike", new List<int>() { 0, 0, 0, 0, 90, 0 } } // R, R, R, R, D, D
        };

        public bool IsActive { get; private set; } = false;
        private Entity player;

        public SwordSkillsManager(Entity player)
        {
            this.player = player;
        }

        public void Activate()        
        {
            this.IsActive = true;
        }

        public void Deactivate()
        {
            this.IsActive = false;
        }

        public void Draw(SadConsole.Console console)
        {
            if (!this.IsActive)
            {
                return;
            }

            var tiles = this.GetSkillTiles("L-Strike", 0);
            foreach (var tile in tiles) {
                console.DrawCharacter(tile.Item1, tile.Item2, '*', Palette.PaleYellow);
            }
        }

        /// <summary>
        /// Get the list of tiles affected if the player uses the specified skill facing the specified direction.
        /// cacingDirectionDegrees = 0 means pointing right.
        /// </summary>
        public IEnumerable<Tuple<int, int>> GetSkillTiles(string skillName, int facingDirectionDegrees)
        {
            var toReturn = new List<Tuple<int, int>>();

            var skillSteps = new List<int>(this.SwordSkillSteps[skillName]);
            var previousFacing = facingDirectionDegrees;
            var currentPosition = new Tuple<int, int>(player.X, player.Y);

            toReturn.Add(currentPosition);
            while (skillSteps.Any())
            {
                var nextStep = skillSteps.ElementAt(0);
                skillSteps.RemoveAt(0);

                var newPosition = CalculateMove(currentPosition, previousFacing, nextStep);
                toReturn.Add(newPosition);

                previousFacing = nextStep;
                currentPosition = newPosition;
            }

            return toReturn;
        }

        private Tuple<int, int> CalculateMove(Tuple<int, int> currentPosition, int previousFacing, int nextStep)
        {
            // Calculate the new angle relative to the old one (eg. 90 => 180 is just a change of 90 degrees)
            var angle = nextStep - previousFacing;
            var radians = DegreeToRadian(angle);
            // Rotate the current position. Take the line from (0, 0) to (0, 1) (pointing up), rotate, apply component diff.
            var dx = (int)Math.Ceiling(Math.Cos(radians));
            var dy = (int)Math.Ceiling(Math.Sin(radians));

            return new Tuple<int, int>(currentPosition.Item1 + dx, currentPosition.Item2 + dy);
        }

        private double DegreeToRadian(double radians)
        {
            return Math.PI * radians / 180.0;
        }
    }
}