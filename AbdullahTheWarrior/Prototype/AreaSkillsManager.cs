using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SadConsole;

namespace DeenGames.AbdullahTheWarrior.Prototype
{
    /// <summary>
    /// Small little class to handle Ghazi sword-skills drawing/input/etc.
    /// </summary>
    public class AreaSkillsManager
    {
        public enum AreaSkill {
            Whirlwind, // Area around you
            PlusStrike, // Hits a bunch of enemies in a +
        }

        /// <summary>
        /// Dictionary of skill => list of coordinates. [0, 0] is player spot; [0, -1] is above him.
        /// </summary>
        private Dictionary<AreaSkill, List<Point>> AreaSkillTiles = new Dictionary<AreaSkill, List<Point>>()
        {
            { AreaSkill.Whirlwind, new List<Point>() { 
                // Rotates clockwise
                new Point(-1, -1), new Point(0, -1), new Point(1, -1), new Point(1, 0), new Point(1, 1), new Point(0, 1), new Point(-1, 1), new Point(-1, 0) 
            }},
            { AreaSkill.PlusStrike, new List<Point>() {
                // Up, right, down, left
                new Point(0, -1), new Point(0, -2), new Point(1, 0), new Point(2, 0), new Point(0, 1), new Point(0, 2), new Point(-1, 0), new Point(-2, 0)
            }},
        };

        public bool IsActive { get; private set; } = false;
        private Entity player;
        private IEnumerable<Entity> monsters;
        private IEnumerable<Vector2> walls;
        private AreaSkill currentSkill;

        public AreaSkillsManager(Entity player, IEnumerable<Entity> monsters, IEnumerable<Vector2> walls)
        {
            this.player = player;

            // for drawing
            this.monsters = monsters;
            this.walls = walls;
        }

        public void Activate(AreaSkill currentSkill)        
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
                char character = '*';
                Color colour = Palette.PaleYellow;

                if (this.monsters.Any(m => m.X == tile.X && m.Y == tile.Y))
                {
                    character = 'X';
                    colour = Palette.Red;
                }
                else if (this.walls.Any(w => w.X == tile.X && w.Y == tile.Y))
                {
                    character = 'X';
                    colour = Palette.White;
                }

                console.DrawCharacter(tile.X, tile.Y, character, colour);
            }
        }

        public void ProcessPlayerInput()
        {
            if (Global.KeyboardState.IsKeyPressed(Keys.NumPad1))
            {
                this.currentSkill = AreaSkill.Whirlwind;
            }
            else if (Global.KeyboardState.IsKeyPressed(Keys.NumPad2))
            {
                this.currentSkill = AreaSkill.PlusStrike;
            }
        }

        /// <summary>
        /// Get the list of tiles affected if the player uses the specified skill.
        /// </summary>
        public IEnumerable<Point> GetSkillTiles()
        {
            var toReturn = new List<Point>();

            foreach (var point in this.AreaSkillTiles[this.currentSkill]) {
                toReturn.Add(new Point(player.X + point.X, player.Y + point.Y));
            }

            return toReturn;
        }

        public int GetCurrentSkillDamage()
        {
            switch (this.currentSkill) {
                case AreaSkill.Whirlwind:
                    return (int)Math.Ceiling(this.player.Strength * 1.2f);
                case AreaSkill.PlusStrike:
                    return (int)Math.Ceiling(this.player.Strength * 0.8f);
            }
    
            throw new InvalidOperationException($"Damage formula  implemented for {this.currentSkill}!");
        }
    }
}