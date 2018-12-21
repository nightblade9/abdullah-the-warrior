using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using SadConsole;

namespace DeenGames.AbdullahTheWarrior.Prototype
{
    /// <summary>
    /// Small little class to handle Ghazi sword-skills drawing/input/etc.
    /// </summary>
    public class SwordSkillsManager
    {
        public enum Skill {
            LStrike,
        }

        /// <summary>
        /// Dictionary of skill => list of steps. Each step is a clockwise rotation value (in degrees).
        /// Eg. if the player is facing right, 0/0/0/90/90 is R/R/R/D/D
        /// </summary>
        private Dictionary<Skill, List<int>> SwordSkillSteps = new Dictionary<Skill, List<int>>()
        {
            { Skill.LStrike, new List<int>() { 0, 0, 0, 0, 90, 90 } } // R, R, R, R, D, D
        };

        public bool IsActive { get; private set; } = false;
        private Entity player;
        private int anglePlayerFacing = 0; // right
        private Skill currentSkill;

        public SwordSkillsManager(Entity player)
        {
            this.player = player;
        }

        public void Activate(Skill currentSkill)        
        {
            this.IsActive = true;
            this.currentSkill = currentSkill;
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

            var tiles = this.GetSkillTiles();
            foreach (var tile in tiles) {
                console.DrawCharacter(tile.Item1, tile.Item2, '*', Palette.PaleYellow);
            }
        }

        public void ProcessPlayerInput()
        {
            if (Global.KeyboardState.IsKeyPressed(Keys.Right) || Global.KeyboardState.IsKeyPressed(Keys.D))
            {
                this.anglePlayerFacing = 0; 
            }
            else if (Global.KeyboardState.IsKeyPressed(Keys.Down) || Global.KeyboardState.IsKeyPressed(Keys.S))
            {
                this.anglePlayerFacing = 90;
            }
            else if (Global.KeyboardState.IsKeyPressed(Keys.Left) || Global.KeyboardState.IsKeyPressed(Keys.A))
            {
                this.anglePlayerFacing = 180;
            }
            else if (Global.KeyboardState.IsKeyPressed(Keys.Up) || Global.KeyboardState.IsKeyPressed(Keys.W))
            {
                this.anglePlayerFacing = 270;
            }
        }

        /// <summary>
        /// Get the list of tiles affected if the player uses the specified skill facing the specified direction.
        /// cacingDirectionDegrees = 0 means pointing right.
        /// </summary>
        public IEnumerable<Tuple<int, int>> GetSkillTiles()
        {
            var toReturn = new List<Tuple<int, int>>();

            var skillSteps = new List<int>(this.SwordSkillSteps[this.currentSkill]);
            var previousFacing = this.anglePlayerFacing;
            var currentPosition = new Tuple<int, int>(player.X, player.Y);

            while (skillSteps.Any())
            {
                var nextStep = skillSteps.ElementAt(0);
                skillSteps.RemoveAt(0);

                var relativeToFacing = this.anglePlayerFacing + nextStep;
                var newPosition = CalculateMove(currentPosition, previousFacing, relativeToFacing);
                toReturn.Add(newPosition);

                previousFacing = nextStep;
                currentPosition = newPosition;
            }

            return toReturn;
        }

        private Tuple<int, int> CalculateMove(Tuple<int, int> currentPosition, int previousFacing, int nextStep)
        {
            // Calculate the new angle relative to the old one (eg. 90 => 180 is just a change of 90 degrees)
            var radians = DegreeToRadian(nextStep);
            // Rotate the current position. Take the line from (0, 0) to (0, 1) (pointing up), rotate, apply component diff.
            var dx = (int)Math.Round(Math.Cos(radians));
            var dy = (int)Math.Round(Math.Sin(radians));

            return new Tuple<int, int>(currentPosition.Item1 + dx, currentPosition.Item2 + dy);
        }

        private double DegreeToRadian(double radians)
        {
            return Math.PI * radians / 180.0;
        }
    }
}